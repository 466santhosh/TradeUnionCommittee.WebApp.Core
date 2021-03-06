﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeUnionCommittee.BLL.ActualResults;
using TradeUnionCommittee.BLL.Configurations;
using TradeUnionCommittee.BLL.DTO.Employee;
using TradeUnionCommittee.BLL.Enums;
using TradeUnionCommittee.BLL.Helpers;
using TradeUnionCommittee.BLL.Interfaces.Lists.Employee;
using TradeUnionCommittee.DAL.EF;
using TradeUnionCommittee.DAL.Entities;

namespace TradeUnionCommittee.BLL.Services.Lists.Employee
{
    internal class FluorographyEmployeesService : IFluorographyEmployeesService
    {
        private readonly TradeUnionCommitteeContext _context;
        private readonly AutoMapperConfiguration _mapperService;
        private readonly HashIdConfiguration _hashIdUtilities;

        public FluorographyEmployeesService(TradeUnionCommitteeContext context, AutoMapperConfiguration mapperService, HashIdConfiguration hashIdUtilities)
        {
            _context = context;
            _mapperService = mapperService;
            _hashIdUtilities = hashIdUtilities;
        }

        public async Task<ActualResult<IEnumerable<FluorographyEmployeesDTO>>> GetAllAsync(string hashIdEmployee)
        {
            try
            {
                var id = _hashIdUtilities.DecryptLong(hashIdEmployee);
                var fluorography = await _context.FluorographyEmployees
                    .Where(x => x.IdEmployee == id)
                    .OrderByDescending(x => x.DatePassage)
                    .ToListAsync();
                var result = _mapperService.Mapper.Map<IEnumerable<FluorographyEmployeesDTO>>(fluorography);
                return new ActualResult<IEnumerable<FluorographyEmployeesDTO>> { Result = result };
            }
            catch (Exception exception)
            {
                return new ActualResult<IEnumerable<FluorographyEmployeesDTO>>(DescriptionExceptionHelper.GetDescriptionError(exception));
            }
        }

        public async Task<ActualResult<FluorographyEmployeesDTO>> GetAsync(string hashId)
        {
            try
            {
                var id = _hashIdUtilities.DecryptLong(hashId);
                var fluorography = await _context.FluorographyEmployees.FindAsync(id);
                if (fluorography == null)
                {
                    return new ActualResult<FluorographyEmployeesDTO>(Errors.TupleDeleted);
                }
                var result = _mapperService.Mapper.Map<FluorographyEmployeesDTO>(fluorography);
                return new ActualResult<FluorographyEmployeesDTO> { Result = result };
            }
            catch (Exception exception)
            {
                return new ActualResult<FluorographyEmployeesDTO>(DescriptionExceptionHelper.GetDescriptionError(exception));
            }
        }

        public async Task<ActualResult> CreateAsync(FluorographyEmployeesDTO item)
        {
            try
            {
                await _context.FluorographyEmployees.AddAsync(_mapperService.Mapper.Map<FluorographyEmployees>(item));
                await _context.SaveChangesAsync();
                return new ActualResult();
            }
            catch (Exception exception)
            {
                return new ActualResult(DescriptionExceptionHelper.GetDescriptionError(exception));
            }
        }

        public async Task<ActualResult> UpdateAsync(FluorographyEmployeesDTO item)
        {
            try
            {
                _context.Entry(_mapperService.Mapper.Map<FluorographyEmployees>(item)).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return new ActualResult();
            }
            catch (Exception exception)
            {
                return new ActualResult(DescriptionExceptionHelper.GetDescriptionError(exception));
            }
        }

        public async Task<ActualResult> DeleteAsync(string hashId)
        {
            try
            {
                var id = _hashIdUtilities.DecryptLong(hashId);
                var result = await _context.FluorographyEmployees.FindAsync(id);
                if (result != null)
                {
                    _context.FluorographyEmployees.Remove(result);
                    await _context.SaveChangesAsync();
                }
                return new ActualResult();
            }
            catch (Exception exception)
            {
                return new ActualResult(DescriptionExceptionHelper.GetDescriptionError(exception));
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        //--------------- Business Logic ---------------
    }
}