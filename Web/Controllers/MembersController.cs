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

		[HttpPost]
		public ActionResult New(NewMember data) {
			var newMember = new Member {
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

			member.Name = data.Name;
			member.Email = data.Email;
			member.LastUpdated = DateTime.Now;

			_context.SaveChanges();

			return Json(JsonConvert.SerializeObject(data));
		}
	}
}