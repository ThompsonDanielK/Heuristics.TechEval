using System.Linq;
using System.Web.Mvc;
using Heuristics.TechEval.Core;
using Heuristics.TechEval.Web.Models;
using Heuristics.TechEval.Core.Models;
using Newtonsoft.Json;
using System;

namespace Heuristics.TechEval.Web.Controllers {

	public class MembersController : Controller {

		private readonly DataContext _context;

		public MembersController() {
			_context = new DataContext();
		}

		public ActionResult List() {
			var allMembers = _context.Members.ToList();

			return View(allMembers);
		}

        public ActionResult New(NewMember data)
        {
            bool isUnique = IsEmailUnique(data.Email);
            if (!isUnique)
            {
                return new HttpStatusCodeResult(409, "Email is taken");
            }

            var newMember = new Member
            {
                Name = data.Name,
                Email = data.Email,
                LastUpdated = DateTime.Now
            };

            _context.Members.Add(newMember);
            _context.SaveChanges();

            return Json(JsonConvert.SerializeObject(newMember));
        }

        [HttpPost]
        public ActionResult Edit(Member data, int id)
        {
            Member member = _context.Members.Find(id);

            if (member == null)
            {
                return new HttpStatusCodeResult(400, "Member not found");
            }

            bool isUnique = IsEmailUnique(data.Email);
            if (!isUnique)
            {
                return new HttpStatusCodeResult(409, "Email is taken");
            }


            member.Name = data.Name;
            member.Email = data.Email;
            member.LastUpdated = DateTime.Now;

            _context.SaveChanges();

            return Json(JsonConvert.SerializeObject(data));
        }

        private bool IsEmailUnique(string email, int memberIdToExclude = 0)
        {
			if (memberIdToExclude == 0)
			{
				return !_context.Members.Any(member => member.Email == email);
			}
			else
			{
				return !_context.Members.Any(member => member.Email == email && member.Id != memberIdToExclude);
			}
		}
	}
}