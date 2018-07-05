﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TradeUnionCommittee.Web.GUI.DropDownLists
{
    public interface IDropDownList
    {
        Task<SelectList> GetLevelEducation();
        Task<SelectList> GetStudy();
        Task<SelectList> GetMainSubdivision();
        //Task<List<Subdivision>> GetSubordinateSubdivision(int id);
        Task<SelectList> GetPosition();
        Task<SelectList> GetDormitory();
        Task<SelectList> GetDepartmental();
        Task<SelectList> GetScientificTitle();
        Task<SelectList> GetAcademicDegree();
        Task<SelectList> GetSocialActivity();
        Task<SelectList> GetPrivilegies();
        Task<SelectList> GetHobby();
        Task<SelectList> GetTravel();
        Task<SelectList> GetWellness();
        Task<SelectList> GetTour();
        Task<SelectList> GetCultural();
        Task<SelectList> GetActivities();
    }
}