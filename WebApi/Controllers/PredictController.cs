using ConsoleApp2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SkiaSharp;
using System.IO;
using System.Text;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictController : ControllerBase
    {
        [HttpPost]
        public string Post(IFormFile formFile)
        {
            try
            {
                var ms = formFile.OpenReadStream();
                byte[] bytes = new byte[ms.Length];
                ms.Read(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                var c = Task.Run(() =>
            {
                var input = new ConsoleApp2.MLModel1.ModelInput()
                {
                    ImageSource = bytes,
            };
                var predictionResult = MLModel1.Predict(input);
                return Task.FromResult(predictionResult.PredictedLabel);
            });
                return c.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "-1";
            }
        }
    }
}
