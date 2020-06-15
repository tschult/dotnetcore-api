using System;
using System.Collections.Generic;
using System.Linq;
using Elo.Api.Dummy.FakeData;
using Elo.Api.Dummy.Models;
using Microsoft.AspNetCore.Mvc;

namespace Elo.Api.Dummy.Controllers
{
    [ApiController]
    [Route("[controller]", Name = "GetRequests")]
    public class RequestsController : ControllerBase
    {
        private readonly Generator _generator;

        public RequestsController(Generator generator)
        {
            _generator = generator;
        }

        [HttpGet]
        public PatientComplexResponse Get(Technique? technique = null,
                                          Sex? sex = null, 
                                          int limit = 20, 
                                          int start = 0, 
                                          string q = "",
                                          string sortOrder = "")
        {
            IEnumerable<RequestPatientComplex> result = _generator.TestData;
            if (technique.HasValue)
            {
                result = result.Where(x => x.Technique == technique.Value);
            }

            if (sex.HasValue)
            {
                result = result.Where(x => x.Sex == sex.Value);
            }

            if (!string.IsNullOrEmpty(q))
            {
                result = result.Where(x => 
                                           x.PatientLastName.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                                           x.PatientFirstName.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                                           x.Comment.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                                           x.CurrentOfficialResultRemark.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                                           x.CurrentDetailedResultString.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    result = result.OrderByDescending(x => x.PatientLastName);
                    break;
                case "lpatid":
                    result = result.OrderBy(x => x.LifelongPatientId);
                    break;
                case "lpatid_desc":
                    result = result.OrderByDescending(x => x.LifelongPatientId);
                    break;
                default:
                    result = result.OrderBy(x => x.PatientLastName);
                    break;
            }

            var resultList = result.ToList();
            var page = resultList.Skip(start).Take(limit).ToList();
            
            
            var links = new Dictionary<string, string>();
            var hasNext = resultList.Count > start + limit;
            if (hasNext)
            {
                links.Add("next", Url.Link("GetRequests",
                    new
                    {
                        q,
                        technique,
                        sex,
                        limit,
                        start = start + limit,
                        sortOrder
                    }));
            }

            var hasPrev = start > 0;
            if (hasPrev)
            {
                links.Add("prev", Url.Link("GetRequests",
                    new
                    {
                        q,
                        technique,
                        sex,
                        limit,
                        start = Math.Max(0, start - limit),
                        sortOrder
                    }));
            }

            return new PatientComplexResponse
            {
                Links = links,
                Limit = limit,
                Start = start,
                Results = page,
                Size = page.Count
        };
        }
    }

    public class PatientComplexResponse
    {
        public IDictionary<string, string> Links { get; set; }
        public int Limit { get; set; }
        public int Size { get; set; }
        public int Start { get; set; }
        public IEnumerable<RequestPatientComplex> Results { get; set; }
    }
}