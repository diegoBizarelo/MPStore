using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MPStore.Core.Communication;

namespace MPStore.WebAPI.Core.Controllers
{
    [ApiController]
    public class MainController : Controller
    {
        protected ICollection<string> Erros = new List<string>();

        protected bool ValidOperations()
        {
            return !Erros.Any();
        }
        protected ActionResult CustomResponse(object? result = null)
        {
            if (ValidOperations())
            {
                return Ok(result);
            }

            return BadRequest(new HttpValidationProblemDetails(new Dictionary<string, string[]>
            {
                {"Mensagens", Erros.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddErroParaPilha(error.ErrorMessage);
            }

            return CustomResponse();
        }

        protected void AddErroParaPilha(string error)
        {
            Erros.Add(error);
        }

        protected void CleanErrors()
        {
            Erros.Clear();
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState) 
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                AddErroParaPilha(erro.ErrorMessage);
            }

            return CustomResponse();
        }

       protected ActionResult CustomResponse(ResponseResult responseResult)
        {
            RespostaContemErros(responseResult);

            return CustomResponse();
        }

        protected bool RespostaContemErros(ResponseResult responseResult)
        {
            if (responseResult is null || !responseResult.Errors.Messages.Any()) 
                return false;

            foreach(var errorMessage in responseResult.Errors.Messages)
            {
                AddErroParaPilha(errorMessage);
            }

            return true;
        }
    }
}
