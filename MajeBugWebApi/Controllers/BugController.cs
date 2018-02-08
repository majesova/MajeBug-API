using MajeBug.Data;
using MajeBug.Data.Repositories;
using MajeBug.Domain;
using MajeBugWebApi.Helpers;
using MajeBugWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

namespace MajeBugWebApi.Controllers
{
    [Authorize]
    public class BugController : BaseApi
    {
        /*DataContext context = new DataContext();
        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }*/

        /// <summary>
        /// GET /api/bug
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<BugApi>))]
        public IHttpActionResult Get()
        {
            using (var context = new DataContext()) {
                BugRepository bugRepository = new BugRepository(context);
                var bugs = bugRepository.GetAll();
                var models = MapperHelper.Map<ICollection<BugApi>>(bugs);
                return Ok(models);
            }
        }

        /// <summary>
        /// Returns a single bug record
        /// </summary>
        /// <param name="id">bug identifier</param>
        /// <returns>Bug instance</returns>
        [ResponseType(typeof(BugApi))]
        public IHttpActionResult Get(int id)
        {
            using (var context = new DataContext())
            {
                BugRepository bugRepository = new BugRepository(context);
                var bug = bugRepository.Find(id);
                UserRepository userRepository = new UserRepository(context);
                bug.CreatedBy = userRepository.Find(bug.CreatedById);
                if (bug.ModifiedById != null) {
                    bug.ModifiedBy = userRepository.Find(bug.ModifiedById);
                }
                var model = MapperHelper.Map<BugApi>(bug);
                return Ok(model);
            }

        }

        // POST: api/Bug
        [ResponseType(typeof(BugApi))]
        public IHttpActionResult Post([FromBody]CreateBugApi model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using (DataContext dataContext = new DataContext()) {
                BugRepository bugRepository = new BugRepository(dataContext);
                var bug = MapperHelper.Map<Bug>(model);
                bug.CreatedAt = DateTime.Now;
                bug.CreatedById = CurrentUserId;
                bugRepository.Insert(bug);
                dataContext.SaveChanges();
                var bugApi = MapperHelper.Map<BugApi>(bug);
                return Ok(bugApi);
            }

        }

        // PUT: api/Bug/5
        public IHttpActionResult Put(int id, [FromBody]BugApi model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                using (DataContext context = new DataContext()) {

                    BugRepository bugRepository = new BugRepository(context);
                    var bug = MapperHelper.Map<Bug>(model);
                    bug.ModifiedAt = DateTime.Now;
                    bug.ModifiedById = CurrentUserId;
                    bugRepository.Update(bug);
                    context.SaveChanges();
                    var bugApi = MapperHelper.Map<BugApi>(bug);
                    return Ok(bugApi);
                }
            }
            catch (DbUpdateConcurrencyException ex) {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Conflict, new { Message = "El registro ha sido modificado" }));
            }

        }

        // DELETE: api/Bug/5
        public void Delete(int id)
        {
        }
    }
}
