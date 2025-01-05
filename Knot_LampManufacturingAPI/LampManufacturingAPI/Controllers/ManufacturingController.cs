
using LampManufacturingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LampManufacturingAPI.Controllers
{

    // [Route("api/[controller]")]
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class ManufacturingController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public ManufacturingController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddConfiguration([FromBody] Configuration config)
        {
            if (config == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(config.OrderNumber))
            {
                return BadRequest("Invalid");
            }

            var parameter = new Configuration
            {
                ConfigurationId = config.ConfigurationId,
                SimulationTime = config.SimulationTime,
                StationName = config.StationName,
                OrderNumber = config.OrderNumber,
                OrderQuantity = config.OrderQuantity,
                OrderAmount = config.OrderAmount,
                ProductName = config.ProductName,
                BatchNumber = config.BatchNumber,
                FinishedGoodsTrayMaxQty = config.FinishedGoodsTrayMaxQty,
                PartName1 = config.PartName1,
                PartName2 = config.PartName2,
                PartName3 = config.PartName3,
                PartName4 = config.PartName4,
                PartName5 = config.PartName5,
                PartName6 = config.PartName6,
                ReplQtyPart1 = config.ReplQtyPart1,
                ReplQtyPart2 = config.ReplQtyPart2,
                ReplQtyPart3 = config.ReplQtyPart3,
                ReplQtyPart4 = config.ReplQtyPart4,
                ReplQtyPart5 = config.ReplQtyPart5,
                ReplQtyPart6 = config.ReplQtyPart6,
                PartThresholdQty = config.PartThresholdQty,
                EmployeeName = config.EmployeeName,
                EmployeeSkillLevel = config.EmployeeSkillLevel
            };

            _appDbContext.Configurations.Add(parameter);
            await _appDbContext.SaveChangesAsync();

            return Ok(parameter);
        }

        [HttpGet]
        public async Task<ActionResult<List<Configuration>>> GetConfigs()
        {
            var configs = _appDbContext.Configurations.ToList();
            return Ok(configs);
        }

        [HttpPost]
        public async Task<IActionResult> AddSimulationData([FromBody] Configuration config)
        {
            if (config == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(config.OrderNumber))
            {
                return BadRequest("Invalid");
            }

            var para_employee = new Employee
            {
                Name = config.EmployeeName,
                SkillLevel = Convert.ToSingle(config.EmployeeSkillLevel)
            };

            _appDbContext.Employees.Add(para_employee);
            await _appDbContext.SaveChangesAsync();

            return Ok(para_employee);
        }

        [HttpPost]
        public async Task<IActionResult> CallInitialSP([FromBody] string config_id)
        {
            if (config_id == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(config_id))
            {
                return BadRequest("Invalid");
            }
            var sp = _appDbContext.Database.ExecuteSql($"dbo.SP_SimulationSetup @Cid={config_id}");
            // await _appDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ActivateBinSensor([FromBody] string config_id)
        {
            Console.WriteLine(config_id);
            if (config_id == null)
            {
                return NotFound();
            }

            if (config_id == " ")
            {
                return BadRequest("Invalid");
            }
            // List: configid, stationId, simulationStatus
            // stationid
            var station = _appDbContext.WorkstationJobs.Where(wsj => wsj.ConfigurationId == config_id).Select(wsj => wsj.StationId).FirstOrDefault();
            var bin = _appDbContext.Bins.Where(b => b.StationId == station).ToList();
            var tray = _appDbContext.Trays.Where(t => t.StationId == station).ToList();
            var emptybin = _appDbContext.Bins.Where(b => (b.StationId == station) && (b.PartQuantity <= 0)).FirstOrDefault();

            if (bin != null && tray != null)
            {
                if (emptybin == null)
                {
                    bin.ForEach(b => b.PartQuantity -= 1);
                    await _appDbContext.SaveChangesAsync();
                    tray.ForEach(t => t.ProductQuantity += 1);
                    await _appDbContext.SaveChangesAsync();
                    return Ok();
                }
                else 
                {
                    return Ok($"{config_id}-station has empty bin(qty0). cannot decrease the bin qty in the database.");
                }
            }
            else
            {
                return BadRequest("Invalid");
            }

        }

        [HttpPost]
        public async Task<IActionResult> ActivateBinButton([FromBody] string config_id)
        {
            // config_id = runnerCentralTimer
            if (config_id == null)
            {
                return NotFound();
            }

            if (config_id == " ")
            {
                return BadRequest("Invalid");
            }

            // get Isreplenishmentneeded bin for the running station
            //var station = _appDbContext.WorkstationJobs.Where(wsj => wsj.Status == "InProgress").Select(wsj => wsj.StationId).FirstOrDefault();
            //var bin = _appDbContext.Bins.Where(b => (b.StationId == station) && (b.IsReplenishmentNeeded == true)).ToList();
            var bin = _appDbContext.Bins.Where(b => (b.IsReplenishmentNeeded == true)).ToList();
            if (bin != null)
            {
                bin.ForEach(b => b.PartQuantity += b.ReplenishentQty);
                await _appDbContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest("Invalid");
            }

            //var station = _appDbContext.WorkstationJobs.Where(wsj => wsj.ConfigurationId == config_id).Select(wsj => wsj.StationId).FirstOrDefault();
            //var bin = _appDbContext.Bins.Where(b => b.StationId == station).ToList();
            //if (bin != null)
            //{
            //    bin.ForEach(b => b.PartQuantity += b.ReplenishentQty);
            //    await _appDbContext.SaveChangesAsync();
            //    return Ok();
            //}
            //else
            //{
            //    return BadRequest("Invalid");
            //}

        }

        [HttpGet]
        public async Task<ActionResult<List<BinsWithWorkstationjob>>> GetViewBinWithWorkstation()
        {
            var viewBin = _appDbContext.BinsWithWorkstationjobs.ToList();
            return Ok(viewBin);
        }

        [HttpGet]
        public async Task<ActionResult<List<WorkstationJob>>> GetWorkstationjob()
        {
            var configs = _appDbContext.WorkstationJobs.ToList();
            return Ok(configs);
        }
    }
}