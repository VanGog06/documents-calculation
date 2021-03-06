﻿using DocumentsCalculation.Models;
using DocumentsCalculation.Services.Constracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentsCalculation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsCalculationController : ControllerBase
    {
        public readonly ICalculationService calculationService;

        public DocumentsCalculationController(ICalculationService calculationService)
        {
            this.calculationService = calculationService;
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<IEnumerable<CalculateInvoiceOutputModel>>> CalculateInvoice([FromForm] CalculateInvoiceInputModel input)
        {
            try
            {
                IEnumerable<CalculateInvoiceOutputModel> result = await this.calculationService.CalculateDocumentsAsync(input);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}