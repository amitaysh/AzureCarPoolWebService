using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using dm = TransactionsService.DomainModels;
using TransactionsService.DTO;
using AutoMapper;
using TransactionsService.Services;

namespace TransactionsService.WebControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;

        public TransactionsController(IMapper mapper, ITransactionService transactionService)
        {
            _mapper = mapper;
            _transactionService = transactionService;
        }

        [HttpGet]
        [SwaggerOperation("GetAllTransactions")]
        public async Task<ActionResult<List<Transaction>>> GetTransactionsAsync()
        {
            var transactions = _mapper.Map<List<Transaction>>(await _transactionService.GetTransactionsAsync());
            return Ok(transactions);
        }

        [HttpGet("{transactionId}")]
        [SwaggerOperation("GetTransactionById")]
        public async Task<ActionResult<Transaction>> GetTransactionByIdAsync(string transactionId)
        {
            var transaction = _mapper.Map<Transaction>(await _transactionService.GetTransactionByIdAsync(transactionId));
            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        [HttpPost]
        [SwaggerOperation("AddTransaction")]
        public async Task<ActionResult<Transaction>> AddTransactionAsync(CreateTransaction transaction)
        {
            var dmTransaction = _mapper.Map<dm.Transaction>(transaction);
            return _mapper.Map<Transaction>(await _transactionService.AddTransactionAsync(dmTransaction));
        }

        [HttpPut("/CloseTransaction")]
        [SwaggerOperation("CloseTransaction")]
        public async Task CloseTransactionAsync(CloseTransaction closeTransaction)
        {
            await _transactionService.CloseTransactionAsync(closeTransaction);
        }

        [HttpDelete("{transactionId}")]
        [SwaggerOperation("DeleteTransaction")]
        public async Task DeleteTransactionAsync(string transactionId)
        {
            await _transactionService.DeleteTransactionAsync(transactionId);
        }
    }
}
