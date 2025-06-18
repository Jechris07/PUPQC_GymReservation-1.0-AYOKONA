﻿using AYOKONA.Entities;
using AYOKONA.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AYOKONA.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        // In AdminController.cs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var hashedPassword = model.GetHashedPassword();

            var admin = _context.AdminAccounts
                .FirstOrDefault(a => a.Email == model.Email && a.PasswordHash == hashedPassword);

            if (admin != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.Name),
                    new Claim(ClaimTypes.Email, admin.Email),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                TempData["SuccessMessage"] = "Admin login successful!";
                // This line ensures redirection to AdminDashboard
                return RedirectToAction("AdminDashboard");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult AdminDashboard()
        {
            return View("AdminDashboard");
        }

        public IActionResult AdminManage()
        {
            return View();
        }

<<<<<<< Updated upstream
        public IActionResult AdminProfile()
        {
            return View();
=======
        [HttpPost]
        public async Task<IActionResult> UpdateRequestStatus(int id, string status)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.RequestId == id);
            if (request != null)
            {
                string? newReservationStatus = null;

                if (status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                {
                    request.Status = "approved"; 
                    newReservationStatus = "completed"; 
                }
                else if (status.Equals("Declined", StringComparison.OrdinalIgnoreCase))
                {
                    request.Status = "denied";
                    newReservationStatus = "cancelled";
                }
                else
                {
                    request.Status = status;
                }

                _context.Requests.Update(request);

                // Update the corresponding Reservation status if needed
                if (newReservationStatus != null)
                {
                    var reservation = await _context.Reservations
                        .FirstOrDefaultAsync(r => r.ReservationId == request.ReservationId);

                    if (reservation != null)
                    {
                        reservation.Status = newReservationStatus;
                        _context.Reservations.Update(reservation);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AdminDashboard));
            }
            return RedirectToAction("AdminManage");
        }

        [Authorize]
        [Authorize]
        public IActionResult AdminProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Email claim not found.");
            }

            var admin = _context.AdminAccounts.FirstOrDefault(a => a.Email == email);

            if (admin == null)
            {
                return NotFound("Admin profile not found.");
            }

            var model = new AdminProfileViewModel
            {
                FullName = admin.Name,
                Department = "Physical Education",
                Position = "Gym Administrator",
                Email = admin.Email,
                Campus = "PUP Quezon City",
                Role = "Student"

            };

            return View(model); // Pass admin to the view
>>>>>>> Stashed changes
        }

        public IActionResult AdminEditProfile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
<<<<<<< Updated upstream
}
=======
}

>>>>>>> Stashed changes
