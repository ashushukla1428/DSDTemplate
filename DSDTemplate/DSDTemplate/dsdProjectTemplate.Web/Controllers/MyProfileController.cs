using dsdProjectTemplate.Services.Question;
using dsdProjectTemplate.Services.SendEmail.User;
using dsdProjectTemplate.Services.User;
using dsdProjectTemplate.Services.User.TwoFactorAuthentication;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.User;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Web.Controllers
{
    [Authorize]
    public class MyProfileController : Controller
    {
        // GET: MyProfile
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;
        private readonly ITwoFactorAuthenticationService _twoFactorAuthenticationService;
        public MyProfileController()
        {
            _questionService = new QuestionService();
            _userService = new UserService();
            _twoFactorAuthenticationService = new TwoFactorAuthenticationService();
        }
        public async Task<ActionResult> Index()
        {
            ViewBag.Questiones = await _questionService.GetDropListAsync();
            return View(await _userService.GetMyProfileAsync());
        }
        public async Task<ActionResult> TwoFactorAuthentication()
        {
            
            return View(await _userService.GetMyProfileAsync());
        }
        [HttpPost]
        public async Task<ActionResult> Index(UserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                return Json(await _userService.UpdateMyProfileAsync(model));
            }
            else
            {
                return Json(new ResponseModel { Status = false, Message = ResponseMessages.requiredFields });
            }
            
        }
        [HttpPost]
        public async Task<ActionResult> SendTwoFactorAuthenticationCodeOnEmail()
        {
            return Json(await _twoFactorAuthenticationService.SendEmail_TwoFactorAuthenticationCode_Async());

        }
        [HttpPost]
        public async Task<ActionResult> SendTwoFactorAuthenticationCodeOnPhone()
        {
            return Json(await _twoFactorAuthenticationService.SendMobileNumber_TwoFactorAuthenticationCode_Async());

        }
        [HttpPost]
        public async Task<ActionResult> AddUpdateMobileNumber_TwoFactorAuthentication_Async(string code, string flag)
        {
            return Json(await _twoFactorAuthenticationService.AddUpdateMobileNumber_TwoFactorAuthentication_Async(code, Convert.ToBoolean(flag)));
        }
        [HttpPost]
        public async Task<ActionResult> AddUpdateEmail_TwoFactorAuthentication_Async(string code, string flag)
        {
            return Json(await _twoFactorAuthenticationService.AddUpdateEmail_TwoFactorAuthentication_Async(code, Convert.ToBoolean(flag)));
        }
        public ActionResult ShowwoFactorAuthentication(bool flag,int reqType)
        {
            ViewBag.flag = flag;
            ViewBag.reqType = reqType;
            return View("_showwoFactorAuthentication");
        }
    }
}