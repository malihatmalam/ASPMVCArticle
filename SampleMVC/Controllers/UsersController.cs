using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using System.Text.Json;
using static Dapper.SqlMapper;

namespace SampleMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserBLL _userBLL;
        private readonly IRoleBLL _roleBLL;

        public UsersController(IUserBLL userBLL, IRoleBLL roleBLL)
        {
            _userBLL = userBLL;
            _roleBLL = roleBLL;
        }

        public IActionResult Index()
        {
            var users = _userBLL.GetAll();
            ViewBag.Users = users;

            var roles = _roleBLL.GetAllRoles();
            
            ViewBag.Roles = roles;

            var usersWithRoles = _userBLL.GetAllWithRoles();
            return View(usersWithRoles);
        }

        [HttpPost]
        public IActionResult AddRole(string username, int roleId) 
        {
            if(string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("User is required");
            }
            if (roleId == null)
            {
                throw new ArgumentException("User is required");
            }
            try
            {
                _roleBLL.AddUserToRole(username, roleId);
                TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Add Data Category Success !</div>";
                return RedirectToAction("Index", "Users");
            }
            catch (Exception ex)
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>" + ex.Message + "</div>";
                return View();
            }
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Anda harus login terlebih dahulu !</div>";
                return RedirectToAction("index", "Users");
            }

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var userDto = _userBLL.LoginMVC(loginDTO);
                //simpan username ke session
                var userDtoSerialize = JsonSerializer.Serialize(userDto);
                HttpContext.Session.SetString("user", userDtoSerialize);

                TempData["Message"] = "Welcome " + userDto.Username;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Message = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>" + ex.Message + "</div>";
                return View();
            }
        }

        [HttpPost]
        [Route("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            TempData["message"] = String.Empty;
            return RedirectToAction("Login");

        }

        //register user baru
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserCreateDTO userCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _userBLL.Insert(userCreateDto);
                ViewBag.Message = @"<div class='alert alert-success'><strong>Success!&nbsp;</strong>Registration process successfully !</div>";

            }
            catch (Exception ex)
            {
                ViewBag.Message = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>" + ex.Message + "</div>";
            }

            return View();
        }

        public IActionResult Profile()
        {
            var userDTO = new UserDTO();
            if (HttpContext.Session.GetString("user") == null)
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Anda harus login terlebih dahulu !</div>";
                return RedirectToAction("Login", "Users");
            }
            else {
                userDTO = JsonSerializer.Deserialize<UserDTO>(HttpContext.Session.GetString("user"));
            }
            //var userWithRoles = _userBLL.GetUserWithRoles("ekurniawan");
            ViewBag.Roles = userDTO.Roles;
            return View(userDTO);
        }
    }
}
