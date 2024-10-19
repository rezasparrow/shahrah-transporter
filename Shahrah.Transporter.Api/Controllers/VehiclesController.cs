using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Api.Models;
using Shahrah.Transporter.Application.Lookups.Models;
using Shahrah.Transporter.Application.Vehicles.Commands.AddVehicle;
using Shahrah.Transporter.Application.Vehicles.Commands.AssignDriver;
using Shahrah.Transporter.Application.Vehicles.Commands.EditVehicle;
using Shahrah.Transporter.Application.Vehicles.Commands.RemoveVehicle;
using Shahrah.Transporter.Application.Vehicles.Commands.UnAssignDriver;
using Shahrah.Transporter.Application.Vehicles.Models;
using Shahrah.Transporter.Application.Vehicles.Queries.GetReadyVehiclesLookup;
using Shahrah.Transporter.Application.Vehicles.Queries.GetVehicle;
using Shahrah.Transporter.Application.Vehicles.Queries.GetVehicles;
using System.Net;

namespace Shahrah.Transporter.Api.Controllers;

public class VehiclesController(IMediator mediator, ICurrentUserService currentUserService) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddVehicle([FromBody] VehicleModel viewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var vehicle = new AddVehicleDto
        {
            PlateNumber = new PlateNumberDto
            {
                PartOne = viewModel.PlateNumber.PartOne,
                PartTwo = viewModel.PlateNumber.PartTwo,
                PartThree = viewModel.PlateNumber.PartThree,
                PartFour = viewModel.PlateNumber.PartFour
            },
            Vin = viewModel.VIN,
            SmartCardNumber = viewModel.SmartCardNumber,
            SmartCardExpirationDate = viewModel.SmartCardExpirationDate,
            TruckTypeId = viewModel.TruckTypeId,
            NetLoadingCapacity = viewModel.NetLoadingCapacity,
            GrossLoadingCapacity = viewModel.GrossLoadingCapacity,
            VehicleOptionItems = viewModel.VehicleOptionItems,
            IsTransporterVehicleOwner = viewModel.IsTransporterVehicleOwner,
            OwnerFirstName = viewModel.OwnerFirstName,
            OwnerLastName = viewModel.OwnerLastName,
            OwnerNationalCode = viewModel.OwnerNationalCode
        };

        if (viewModel.IsTransporterVehicleOwner)
        {
            vehicle.OwnerFirstName = null;
            vehicle.OwnerLastName = null;
            vehicle.OwnerNationalCode = null;
        }

        await _mediator.Send(new AddVehicleCommand(vehicle, _currentUserService.Id));
        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VehicleDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var vehicles = await _mediator.Send(new GetVehiclesQuery(_currentUserService.Id));
        return Ok(vehicles);
    }

    [HttpGet("Ready")]
    [ProducesResponseType(typeof(IEnumerable<ReadyVehiclesLookupDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetReady()
    {
        var vehicles = await _mediator.Send(new GetReadyVehiclesLookupQuery(_currentUserService.Id));
        return Ok(vehicles);
    }

    [HttpPatch("{vehicleId:int}/drivers/{driverId:int}/assign")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> AssignDriver(int vehicleId, int driverId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _mediator.Send(new AssignDriverCommand(_currentUserService.Id, vehicleId, driverId));
        return Ok();
    }

    [HttpPatch("{vehicleId:int}/drivers/{driverId:int}/unassign")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> UnAssignDriver(int vehicleId, int driverId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _mediator.Send(new UnAssignDriverCommand(_currentUserService.Id, vehicleId, driverId));
        return Ok();
    }

    [HttpGet("{vehicleId:int}")]
    [ProducesResponseType(typeof(IEnumerable<VehicleForEditDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetVehicle(int vehicleId)
    {
        var vehicles = await _mediator.Send(new GetVehicleQuery(vehicleId, _currentUserService.Id));
        return Ok(vehicles);
    }

    [HttpDelete("{vehicleId}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteVehicle(int vehicleId)
    {
        var vehicles = await _mediator.Send(new RemoveVehicleCommand(_currentUserService.Id, vehicleId));
        return Ok(vehicles);
    }

    [HttpPut("{vehicleId}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateVehicle(int vehicleId, [FromBody] VehicleModel viewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var vehicle = new EditVehicleDto
        {
            VehicleId = vehicleId,
            PlateNumber = viewModel.PlateNumber.GetPlateNumber(),
            Vin = viewModel.VIN,
            SmartCardNumber = viewModel.SmartCardNumber,
            SmartCardExpirationDate = viewModel.SmartCardExpirationDate,
            TruckTypeId = viewModel.TruckTypeId,
            NetLoadingCapacity = viewModel.NetLoadingCapacity,
            GrossLoadingCapacity = viewModel.GrossLoadingCapacity,
            VehicleOptionItems = viewModel.VehicleOptionItems,
            IsTransporterVehicleOwner = viewModel.IsTransporterVehicleOwner,
            OwnerFirstName = viewModel.OwnerFirstName,
            OwnerLastName = viewModel.OwnerLastName,
            OwnerNationalCode = viewModel.OwnerNationalCode
        };

        if (viewModel.IsTransporterVehicleOwner)
        {
            vehicle.OwnerFirstName = null;
            vehicle.OwnerLastName = null;
            vehicle.OwnerNationalCode = null;
        }

        await _mediator.Send(new EditVehicleCommand(vehicle, _currentUserService.Id));
        return Ok();
    }
}