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

        /// <summary>
        /// Constructor that initializes the AddressController with dependencies.
        /// </summary>
        /// <param name="addressService">Service for address operations</param>
        /// <param name="commonService">Common service for shared functionalities</param>
        public AddressController(AddressService addressService, CommonService commonService)
        {
            _addressService = addressService;
            _commonService = commonService;
        }

        /// <summary>
        /// Retrieves the list of addresses for the authenticated user.
        /// </summary>
        /// <returns>ActionResult containing the list of addresses</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var addressListResponse = await _addressService.Get();
                return Ok(addressListResponse);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error retrieving addresses. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves a specific address by its ID for the authenticated user.
        /// </summary>
        /// <param name="id">Address ID</param>
        /// <returns>ActionResult containing the address details</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var addressResponse = await _addressService.Get(id);
                return Ok(addressResponse);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error retrieving address. Please try again later.");
            }
        }

        /// <summary>
        /// Creates a new address for the authenticated user.
        /// </summary>
        /// <param name="dto">Data Transfer Object for address creation</param>
        /// <returns>ActionResult containing the created address details</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddressDTO dto)
        {
            try
            {
                var addressResponse = await _addressService.Post(dto);
                var uri = await _commonService.GetUri(this, addressResponse.Id.ToString());
                return Created(uri, addressResponse);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving address: {error.Message}");
            }
        }

        /// <summary>
        /// Deletes a specific address by its ID for the authenticated user.
        /// </summary>
        /// <param name="id">Address ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _addressService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting address: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a specific address by its ID for the authenticated user.
        /// </summary>
        /// <param name="id">Address ID</param>
        /// <param name="dto">Data Transfer Object for address update</param>
        /// <returns>ActionResult containing the updated address details</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] AddressUpdateDTO dto)
        {
            try
            {
                var addressResponse = await _addressService.Put(id, dto);
                return Ok(addressResponse);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error updating address: " + error.Message);
            }
        }
    }
}
