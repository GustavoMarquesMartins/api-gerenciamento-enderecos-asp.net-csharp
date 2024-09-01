using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Service;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoDeEndereco.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly AddressService _addressService;
        private readonly CommonService _commonService;

        public AddressController(AddressService addressService, CommonService commonService)
        {
            _addressService = addressService;
            _commonService = commonService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var addressListResponse = await _addressService.get();
                return Ok(addressListResponse);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao buscar endereços. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var addressResponse = await _addressService.get(id);
                return Ok(addressResponse);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao buscar endereço. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddressDTO dto)
        {
            try
            {
                var addressResponse = await _addressService.post(dto);
                var uri = await _commonService.getUri(this, addressResponse.Id.ToString());   
                return Created(uri, addressResponse);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao salvar o endereço: {error.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _addressService.delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar o endereço: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] AddressUpdateDTO dto)
        {
            try
            {
                var addressResponse = await _addressService.put(id, dto);
                return Ok(addressResponse);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao tentar atualizar endereço: " + error.Message);
            }
        }
    }
}
