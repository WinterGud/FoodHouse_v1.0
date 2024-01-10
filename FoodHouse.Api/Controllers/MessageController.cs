using System.Net;
using System.Net.Mail;
using FoodHouse.Api.Repository.GenericRepository;
using FoodHouse.UI.Dto;
using FoodHouse.UI.Models.ViewModel.Admin.Messages;
using Furni.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Furni.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessageController : ControllerBase
	{
		private readonly IGenericRepository<Message> _userRepository;

		public MessageController(IGenericRepository<Message> userRepository)
		{
			_userRepository = userRepository;
		}
		[HttpPost]
		[Route("Add")]
		public async Task Add(MessageDto user)
		{
			Message message = new()
			{
				MessageAboutProblem = user.MessageAboutProblem,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
			};

			await _userRepository.Add(message);
		}
		[HttpGet]
		[Route("GetAll")]
		public List<MessageDto> GetAll()
		{
			var data = _userRepository.GetAll().ToList();

			var messages = new List<MessageDto>();
			foreach(var item in data)
			{
				messages.Add(ConvertToDto(item));
			}

			return messages;
		}

		[HttpGet]
		[Route("Get")]
		public async Task<MessageDto> Get(string id)
		{
			var data = await _userRepository.Get(ObjectId.Parse(id));

			var message = ConvertToDto(data);
			return message;
		}

		[HttpDelete]
		[Route("Delete")]
		public async Task Delete(string id)
		{
			await _userRepository.Delete(ObjectId.Parse(id));
		}
		

		[HttpPost]
		[Route("SendAnswear")]
		public async Task SendAnswear(SendAnswearViewModel message)
		{
			var mail = "akozmenchuk@gmail.com";
			var pw = "klqmannycvyuwscz";	
			var subject = "Foodouse - Give you answear";

			var mess = message.Answear;
			var client = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true,
				Credentials = new NetworkCredential(mail, pw),
			};
    
			await client.SendMailAsync(
				new MailMessage(from: mail,
					to: message.Email,
					subject,
					mess));
			
			await Delete(message.Id);
		}

		private MessageDto ConvertToDto(Message item)
		{
			MessageDto messages = new()
			{
				Id = item._id.ToString(),
				FirstName = item.FirstName,
				LastName = item.LastName,
				Email = item.Email,
				MessageAboutProblem = item.MessageAboutProblem
			};
			return messages;
		}
	}
}
