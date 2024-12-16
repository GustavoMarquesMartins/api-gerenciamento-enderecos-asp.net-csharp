using AddressManagement.DTO;
using AddressManagement.Service;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Service;
using Microsoft.AspNetCore.Mvc;

namespace AddressManagement.Controllers
{
    /// <summary>
    /// Provides API endpoints for address-related operations for the authenticated user.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly AddressService _addressService;
        private readonly CommonService _commonService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressController"/> class.
        /// </summary>
        /// <param name="addressService">Service for handling address operations.</param>
        /// <param name="commonService">Service providing common functionalities.</param>
        public AddressController(AddressService addressService, CommonService commonService)
        {
            _addressService = addressService;
            _commonService = commonService;
        }

        /// <summary>
        /// Retrieves the list of all addresses for the authenticated user.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/> containing a list of addresses.</returns>
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal error retrieving all addresses: {error.Message}");
            }
        }

        /// <summary>
        /// Retrieves a specific address by its ID for the authenticated user.
        /// </summary>
        /// <param name="id">The ID of the address to retrieve.</param>
        /// <returns>An <see cref="ActionResult"/> containing the address details.</returns>
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal error retrieving address: {error.Message}");
            }
        }

        /// <summary>
        /// Creates a new address for the authenticated user.
        /// </summary>
        /// <param name="dto">The data transfer object containing the address information.</param>
        /// <returns>An <see cref="ActionResult"/> containing the created address details.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddressDTO dto)
        {
            try
            {
                var addressResponse = await _addressService.Post(dto);
                var uri = _commonService.GetUri(this, addressResponse.Id.ToString());
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
        /// <param name="id">The ID of the address to delete.</param>
        /// <returns>No content on successful deletion.</returns>
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
        /// <param name="id">The ID of the address to update.</param>
        /// <param name="dto">The data transfer object containing the updated address information.</param>
        /// <returns>An <see cref="ActionResult"/> containing the updated address details.</returns>
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal error updating address: {error.Message}");
            }
        }
    }
}
