using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PackagingGenetic;


namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExperimentsController : ControllerBase
    {
        [HttpGet]
        public string GetExperiment()
        {
            return "server is working";
        }

        [HttpGet("{Name}")]
        public ActionResult<string> GetExperiment(string Name)
        {
            if (Name == "all")
            {
                List<string>? Names = SaveLoad.GetAllNames();
                return JsonSerializer.Serialize(Names);
            } else
            {
                Experiment? Ex = SaveLoad.Load(Name);
                if (Ex == null)
                {
                    return StatusCode(404, "Experiment is not found");
                }
                return JsonSerializer.Serialize(Ex.ExPopulation.FirstGen());
            }
        }

        [HttpPut("{cnt1}/{cnt2}/{cnt3}/{popSize}")]
        public string AddExperiment(int cnt1, int cnt2, int cnt3, int popSize)
        {
            List<string>? AllNames = SaveLoad.GetAllNames();
            Experiment NewEx = new(cnt1, cnt2, cnt3, popSize, AllNames);
            SaveLoad.Save(NewEx);
            return NewEx.ExName;
        }

        [HttpPost("{Name}")]
        public ActionResult<string> StepExperiment(string Name) 
        {
            Experiment? Ex = SaveLoad.Load(Name);
            if (Ex == null)
            {
                return StatusCode(404, "Experiment is not found");
            }
            Ex.Step();
            SaveLoad.Update(Ex);
            return JsonSerializer.Serialize(Ex) + '#' + JsonSerializer.Serialize(Ex.ExPopulation.FirstGen());
        }

        [HttpDelete("{Name}")]
        public ActionResult DeleteExperiment(string Name) 
        {
            if (SaveLoad.Delete(Name) == -1)
            {
                return StatusCode(404, "Experiment is not found");
            }
            return StatusCode(200);
        }
    }
}