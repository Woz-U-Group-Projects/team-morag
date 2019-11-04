using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/videos")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public VideosController(IDatingRepository repo, IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetVideo")]
        public async Task<IActionResult> GetVideo(int id)
        {
            var videoFromRepo = await _repo.GetVideo(id);

            var video = _mapper.Map<VideoForReturnDto>(videoFromRepo);

            return Ok(video);
        }

        [HttpPost]
        public async Task<IActionResult> AddVideoForUser(int userId,
            [FromForm]VideoForCreationDto videoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            var file = videoForCreationDto.File;

            var uploadResult = new VideoUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new VideoUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                            .Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            videoForCreationDto.Url = uploadResult.Uri.ToString();
            videoForCreationDto.PublicId = uploadResult.PublicId;

            var video = _mapper.Map<Video>(videoForCreationDto);

            if (!userFromRepo.Videos.Any(u => u.IsMain))
                video.IsMain = true;

            userFromRepo.Videos.Add(video);

            if (await _repo.SaveAll())
            {
                var videoToReturn = _mapper.Map<VideoForReturnDto>(video);
                return CreatedAtRoute("GetVideo", new { id = video.Id }, videoToReturn);
            }

            return BadRequest("Could not add the video");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainVideo(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _repo.GetUser(userId);

            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized();

            var videoFromRepo = await _repo.GetVideo(id);

            if (videoFromRepo.IsMain)
                return BadRequest("This is already the main video");

            var currentMainVideo = await _repo.GetMainVideoForUser(userId);
            currentMainVideo.IsMain = false;

            videoFromRepo.IsMain = true;

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("Could not set video to main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _repo.GetUser(userId);

            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized();

            var videoFromRepo = await _repo.GetVideo(id);

            if (videoFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo");

            if (videoFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(videoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _repo.Delete(videoFromRepo);
                }
            }

            if (videoFromRepo.PublicId == null)
            {
                _repo.Delete(videoFromRepo);
            }

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the video");
        }

    }
}