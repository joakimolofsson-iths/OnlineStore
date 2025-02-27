using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using System.Text.Json;

namespace OnlineStore.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HorrorBooksController : ControllerBase
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly string _jsonFilePath;

		public HorrorBooksController(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
			_jsonFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "horrorBooks.json");
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<HorrorBook>>> Get()
		{
			var horrorBooks = await ReadHorrorBooksFromFile();
			return Ok(horrorBooks);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<HorrorBook>> Get(int id)
		{
			var horrorBooks = await ReadHorrorBooksFromFile();
			var horrorBook = horrorBooks.FirstOrDefault(hb => hb.Id == id);

			if(horrorBook == null)
			{
				return NotFound();
			}

			return Ok(horrorBook);
		}

		private async Task<List<HorrorBook>> ReadHorrorBooksFromFile()
		{
			if(!System.IO.File.Exists(_jsonFilePath))
			{
				return new List<HorrorBook>();
			}

			var jsonData = await System.IO.File.ReadAllTextAsync(_jsonFilePath);
			return JsonSerializer.Deserialize<List<HorrorBook>>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<HorrorBook>();
		}
	}
}
