using DevIO.App.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("error/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            var errorViewModel = new ErrorViewModel();

            if (id == 500)
            {
                errorViewModel.Message = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                errorViewModel.Title = "Ocorreu um erro!";
                errorViewModel.StatusCodeError = id;
            }
            else if (id == 404)
            {
                errorViewModel.Message = "A p�gina que est� procurando n�o existe! <br />Em caso de d�vidas entre em contato com nosso suporte";
                errorViewModel.Title = "Ops! P�gina n�o encontrada.";
                errorViewModel.StatusCodeError = id;
            }
            else if (id == 403)
            {
                errorViewModel.Message = "Voc� n�o tem permiss�o para fazer isto.";
                errorViewModel.Title = "Acesso Negado";
                errorViewModel.StatusCodeError = id;
            }
            else
            {
                return StatusCode(500);
            }

            return View("Error", errorViewModel);
        }

    }
}
