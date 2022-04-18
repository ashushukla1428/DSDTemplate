using EEONow.Interfaces;
using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EEONow.Context;
using System.Configuration;
using System.Web.Mvc;
using EEONow.Context.EntityContext;
using System.Web;
using EEONow.Utilities;
using System.IO;
using SelectPdf;

namespace EEONow.Services
{
    public class AvailableLaborMarketService : IAvailableLaborMarketService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public AvailableLaborMarketService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }

        public ResponseModel AddNewLaborMarketData()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                AvailableLaborMarketFileVersionModel _result = new AvailableLaborMarketFileVersionModel();
                var users = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();

                List<AvailableLaborMarketFileVersion> _AvailableLaborMarketFileVersions = new List<AvailableLaborMarketFileVersion>();
                if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    Int32 adminselectedOrgId = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _AvailableLaborMarketFileVersions = _context.AvailableLaborMarketFileVersions.Where(e => e.Active == true && e.Organization.OrganizationId == adminselectedOrgId).ToList();
                }
                else
                {
                    if (users != null)
                    {
                        _AvailableLaborMarketFileVersions = users.UserRole.Organization.AvailableLaborMarketFileVersions.Where(e => e.Active == true).ToList();
                    }
                }

                foreach (var item in _AvailableLaborMarketFileVersions)
                {
                    item.Active = false;
                }
                _context.SaveChanges();
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = 1 };
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "AddNewLaborMarketData", "AvailableLaborMarketService.cs");
                throw;
            }
        }

        public AvailableLaborMarketFileVersionModel GetAvailableLaborMarketData()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                AvailableLaborMarketFileVersionModel _result = new AvailableLaborMarketFileVersionModel();
                var users = _context.Users.Where(e => e.UserId == _user).FirstOrDefault();
                AvailableLaborMarketFileVersion _AvailableLaborMarketFileVersions = new AvailableLaborMarketFileVersion();
                Organization _SelectedOrd = new Organization();
                Int32 _selectedOrganizationID = 0;

                if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    _selectedOrganizationID = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _SelectedOrd = _context.Organizations.Where(e => e.OrganizationId == _selectedOrganizationID).FirstOrDefault();
                    _AvailableLaborMarketFileVersions = _context.AvailableLaborMarketFileVersions.Where(e => e.Active == true && e.Organization.OrganizationId == _selectedOrganizationID).FirstOrDefault();
                }
                else
                {
                    if (users != null)
                    {
                        _selectedOrganizationID = users.UserRole.Organization.OrganizationId;
                        _SelectedOrd = _context.Organizations.Where(e => e.OrganizationId == _selectedOrganizationID).FirstOrDefault();
                        _AvailableLaborMarketFileVersions = users.UserRole.Organization.AvailableLaborMarketFileVersions.Where(e => e.Active == true).FirstOrDefault();
                    }
                }
                               
                if (_AvailableLaborMarketFileVersions != null)
                {
                    _result.Active = _AvailableLaborMarketFileVersions.Active;
                    _result.AvailableLaborMarketFileVersionId = _AvailableLaborMarketFileVersions.AvailableLaborMarketFileVersionId;
                    _result.FileVersionNumber = _AvailableLaborMarketFileVersions.FileVersionNumber;
                    _result.Notes = _AvailableLaborMarketFileVersions.Notes;
                    _result.OrganizationId = _AvailableLaborMarketFileVersions.Organization.OrganizationId;
                    _result.SubmissionDateTime = _AvailableLaborMarketFileVersions.SubmissionDateTime;

                    List<AvailableLaborMarketEEOJobCategoryModel> _ListAvailableLaborMarketEEOJobCategoryModel = new List<AvailableLaborMarketEEOJobCategoryModel>();
                    var ListEEOJobCategories = _AvailableLaborMarketFileVersions.Organization.EEOJobCategories.Where(e => e.Active == true).ToList();//Where(e => e.Active == true).
                    var ListIncativeEEOCategorgies = _AvailableLaborMarketFileVersions.AvailableLaborMarketDatas.Select(e => e.EEOJobCategory).Distinct().ToList();
                    if (ListEEOJobCategories.Count() != ListIncativeEEOCategorgies.Count())
                    {
                        _result.Active = false;

                        foreach (var itemInactiveEEOJobCategories in ListIncativeEEOCategorgies)
                        {
                            AvailableLaborMarketEEOJobCategoryModel _modelAvailableLaborMarketEEOJobCategory = new AvailableLaborMarketEEOJobCategoryModel();
                            var listRace = _AvailableLaborMarketFileVersions.AvailableLaborMarketDatas.Where(e => e.EEOJobCategory.EEOJobCategoryId == itemInactiveEEOJobCategories.EEOJobCategoryId).ToList();
                            if (listRace.Count() > 0)
                            {
                                _modelAvailableLaborMarketEEOJobCategory.EEOJobCategoryId = itemInactiveEEOJobCategories.EEOJobCategoryId;
                                _modelAvailableLaborMarketEEOJobCategory.EEOJobCategoryName = itemInactiveEEOJobCategories.Name;
                                _modelAvailableLaborMarketEEOJobCategory.EEOJobCategoryNumber = itemInactiveEEOJobCategories.EEOJobCategoryNumber;

                                List<AvailableLaborMarketDataModel> _listAvailableLaborMarketDataModel = new List<AvailableLaborMarketDataModel>();
                                foreach (var _itemRace in listRace)
                                {
                                    AvailableLaborMarketDataModel _modelAvailableLaborMarketData = new AvailableLaborMarketDataModel();
                                    _modelAvailableLaborMarketData.AvailableLaborMarketDataId = _itemRace.AvailableLaborMarketDataId;
                                    _modelAvailableLaborMarketData.EEOJobCategoryId = itemInactiveEEOJobCategories.EEOJobCategoryId;
                                    _modelAvailableLaborMarketData.RaceId = _itemRace.Race.RaceId;
                                    _modelAvailableLaborMarketData.RaceName = _itemRace.Race.Name;
                                    _modelAvailableLaborMarketData.MaleValue = _itemRace.MaleValue;
                                    _modelAvailableLaborMarketData.FemaleValue = _itemRace.FemaleValue;
                                    _listAvailableLaborMarketDataModel.Add(_modelAvailableLaborMarketData);
                                }
                                _modelAvailableLaborMarketEEOJobCategory.ListAvailableLaborMarketData = _listAvailableLaborMarketDataModel;
                            }

                            _ListAvailableLaborMarketEEOJobCategoryModel.Add(_modelAvailableLaborMarketEEOJobCategory);
                        }
                    }
                    else
                    {
                        foreach (var itemEEOJobCategories in ListEEOJobCategories)
                        {
                            AvailableLaborMarketEEOJobCategoryModel _modelAvailableLaborMarketEEOJobCategory = new AvailableLaborMarketEEOJobCategoryModel();
                            var listRace = _AvailableLaborMarketFileVersions.AvailableLaborMarketDatas.Where(e => e.EEOJobCategory.EEOJobCategoryId == itemEEOJobCategories.EEOJobCategoryId).ToList();
                            if (listRace.Count() > 0)
                            {
                                _modelAvailableLaborMarketEEOJobCategory.EEOJobCategoryId = itemEEOJobCategories.EEOJobCategoryId;
                                _modelAvailableLaborMarketEEOJobCategory.EEOJobCategoryName = itemEEOJobCategories.Name;
                                _modelAvailableLaborMarketEEOJobCategory.EEOJobCategoryNumber = itemEEOJobCategories.EEOJobCategoryNumber;

                                List<AvailableLaborMarketDataModel> _listAvailableLaborMarketDataModel = new List<AvailableLaborMarketDataModel>();
                                foreach (var _itemRace in listRace)
                                {
                                    AvailableLaborMarketDataModel _modelAvailableLaborMarketData = new AvailableLaborMarketDataModel();
                                    _modelAvailableLaborMarketData.AvailableLaborMarketDataId = _itemRace.AvailableLaborMarketDataId;
                                    _modelAvailableLaborMarketData.EEOJobCategoryId = itemEEOJobCategories.EEOJobCategoryId;
                                    _modelAvailableLaborMarketData.RaceId = _itemRace.Race.RaceId;
                                    _modelAvailableLaborMarketData.RaceName = _itemRace.Race.Name;
                                    _modelAvailableLaborMarketData.MaleValue = _itemRace.MaleValue;
                                    _modelAvailableLaborMarketData.FemaleValue = _itemRace.FemaleValue;

                                    _listAvailableLaborMarketDataModel.Add(_modelAvailableLaborMarketData);
                                }
                                _modelAvailableLaborMarketEEOJobCategory.ListAvailableLaborMarketData = _listAvailableLaborMarketDataModel;
                            }

                            _ListAvailableLaborMarketEEOJobCategoryModel.Add(_modelAvailableLaborMarketEEOJobCategory);
                        }
                    }
                    _result.ListAvailableLaborMarketEEOJobCategory = _ListAvailableLaborMarketEEOJobCategoryModel;
                }
                else
                {
                    _result.Active = true;
                    _result.AvailableLaborMarketFileVersionId = 0;
                    _result.FileVersionNumber = _AvailableLaborMarketFileVersions == null ? 1 : _AvailableLaborMarketFileVersions.Organization.AvailableLaborMarketFileVersions.Count() + 1;
                    _result.Notes = string.Empty;
                    _result.OrganizationId = _SelectedOrd.OrganizationId;
                    _result.SubmissionDateTime = DateTime.Now;

                    List<AvailableLaborMarketEEOJobCategoryModel> _ListAvailableLaborMarketEEOJobCategoryModel = new List<AvailableLaborMarketEEOJobCategoryModel>();


                    var ListEEOJobCategories = _AvailableLaborMarketFileVersions == null ? _SelectedOrd.EEOJobCategories.Where(e => e.Active == true).ToList() : _AvailableLaborMarketFileVersions.Organization.EEOJobCategories.Where(e => e.Active == true).ToList();

                    foreach (var itemEEOJobCategories in ListEEOJobCategories)
                    {
                        AvailableLaborMarketEEOJobCategoryModel _modelAvailableLaborMarketEEOJobCategory = new AvailableLaborMarketEEOJobCategoryModel();
                        var listRace = _AvailableLaborMarketFileVersions == null ? _SelectedOrd.Races.Where(e => e.Active == true).ToList() : _AvailableLaborMarketFileVersions.Organization.Races.Where(e => e.Active == true).ToList();
                        if (listRace.Count() > 0)
                        {
                            _modelAvailableLaborMarketEEOJobCategory.EEOJobCategoryId = itemEEOJobCategories.EEOJobCategoryId;
                            _modelAvailableLaborMarketEEOJobCategory.EEOJobCategoryName = itemEEOJobCategories.Name;
                            _modelAvailableLaborMarketEEOJobCategory.EEOJobCategoryNumber = itemEEOJobCategories.EEOJobCategoryNumber;

                            List<AvailableLaborMarketDataModel> _listAvailableLaborMarketDataModel = new List<AvailableLaborMarketDataModel>();
                            foreach (var _itemRace in listRace)
                            {
                                AvailableLaborMarketDataModel _modelAvailableLaborMarketData = new AvailableLaborMarketDataModel();
                                _modelAvailableLaborMarketData.AvailableLaborMarketDataId = 0;
                                _modelAvailableLaborMarketData.EEOJobCategoryId = itemEEOJobCategories.EEOJobCategoryId;
                                _modelAvailableLaborMarketData.RaceId = _itemRace.RaceId;
                                _modelAvailableLaborMarketData.RaceName = _itemRace.Name;
                                _modelAvailableLaborMarketData.MaleValue = 0;
                                _modelAvailableLaborMarketData.FemaleValue = 0;
                                _listAvailableLaborMarketDataModel.Add(_modelAvailableLaborMarketData);
                            }
                            _modelAvailableLaborMarketEEOJobCategory.ListAvailableLaborMarketData = _listAvailableLaborMarketDataModel;
                        }

                        _ListAvailableLaborMarketEEOJobCategoryModel.Add(_modelAvailableLaborMarketEEOJobCategory);
                    }
                    _result.ListAvailableLaborMarketEEOJobCategory = _ListAvailableLaborMarketEEOJobCategoryModel;
                }
                return _result;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetAvailableLaborMarketData", "AvailableLaborMarketService.cs");
                throw;
            }
        }

        public AvailableLaborMarketFileVersionModel GetAvailableLaborMarketDataViaFileVersion(int AvailableLaborMarketFileVersionId)
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                AvailableLaborMarketFileVersionModel _result = new AvailableLaborMarketFileVersionModel();
                var users = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                Int32 _selectedOrganizationID = 0;

                AvailableLaborMarketFileVersion _AvailableLaborMarketFileVersions = new AvailableLaborMarketFileVersion();
                if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    _selectedOrganizationID = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());

                    _AvailableLaborMarketFileVersions = _context.AvailableLaborMarketFileVersions.Where(e => e.AvailableLaborMarketFileVersionId == AvailableLaborMarketFileVersionId && e.Organization.OrganizationId == _selectedOrganizationID).FirstOrDefault();
                }
                else
                {
                    if (users != null)
                    {
                        _AvailableLaborMarketFileVersions = users.UserRole.Organization.AvailableLaborMarketFileVersions.Where(e => e.AvailableLaborMarketFileVersionId == AvailableLaborMarketFileVersionId).FirstOrDefault();
                    }
                }

                if (_AvailableLaborMarketFileVersions != null)
                {
                    _result.Active = _AvailableLaborMarketFileVersions.Active;
                    _result.AvailableLaborMarketFileVersionId = _AvailableLaborMarketFileVersions.AvailableLaborMarketFileVersionId;
                    _result.FileVersionNumber = _AvailableLaborMarketFileVersions.FileVersionNumber;
                    _result.Notes = _AvailableLaborMarketFileVersions.Notes;
                    _result.OrganizationId = _AvailableLaborMarketFileVersions.Organization.OrganizationId;
                    _result.SubmissionDateTime = _AvailableLaborMarketFileVersions.SubmissionDateTime;

                    List<AvailableLaborMarketEEOJobCategoryModel> _ListAvailableLaborMarketEEOJobCategoryModel = new List<AvailableLaborMarketEEOJobCategoryModel>();
                    var ListEEOJobCategories = _AvailableLaborMarketFileVersions.Organization.EEOJobCategories.Where(e => e.Active == true).ToList();//
                    var ListIncativeEEOCategorgies = _AvailableLaborMarketFileVersions.AvailableLaborMarketDatas.Select(e => e.EEOJobCategory).Distinct().ToList();
                    if (ListEEOJobCategories.Count() != ListIncativeEEOCategorgies.Count())
                    {
                        _result.Active = false;
                        foreach (var itemInactiveEEOJobCategories in ListIncativeEEOCategorgies)
                        {
                            AvailableLaborMarketEEOJobCategoryModel _modelAvailableLaborMarketEEOJobCategory = new AvailableLaborMarketEEOJobCategoryModel();
                            var listRace = _AvailableLaborMarketFileVersions.AvailableLaborMarketDatas.Where(e => e.EEOJobCategory.EEOJobCategoryId == itemInactiveEEOJobCategories.EEOJobCategoryId).ToList();
                            if (listRace.Count() > 0)
                            {
                                _modelAvailableLaborMarketEEOJobCategory.EEOJobCategoryId = itemInactiveEEOJobCategories.EEOJobCategoryId;
                                _modelAvailableLaborMarketEEOJobCategory.EEOJobCategoryName = itemInactiveEEOJobCategories.Name;
                                _modelAvailableLaborMarketEEOJobCategory.EEOJobCategoryNumber = itemInactiveEEOJobCategories.EEOJobCategoryNumber;

                                List<AvailableLaborMarketDataModel> _listAvailableLaborMarketDataModel = new List<AvailableLaborMarketDataModel>();
                                foreach (var _itemRace in listRace)
                                {
                                    AvailableLaborMarketDataModel _modelAvailableLaborMarketData = new AvailableLaborMarketDataModel();
                                    _modelAvailableLaborMarketData.AvailableLaborMarketDataId = _itemRace.AvailableLaborMarketDataId;
                                    _modelAvailableLaborMarketData.EEOJobCategoryId = itemInactiveEEOJobCategories.EEOJobCategoryId;
                                    _modelAvailableLaborMarketData.RaceId = _itemRace.Race.RaceId;
                                    _modelAvailableLaborMarketData.RaceName = _itemRace.Race.Name;
                                    _modelAvailableLaborMarketData.MaleValue = _itemRace.MaleValue;
                                    _modelAvailableLaborMarketData.FemaleValue = _itemRace.FemaleValue;
                                    _listAvailableLaborMarketDataModel.Add(_modelAvailableLaborMarketData);
                                }
                                _modelAvailableLaborMarketEEOJobCategory.ListAvailableLaborMarketData = _listAvailableLaborMarketDataModel;
                            }

                            _ListAvailableLaborMarketEEOJobCategoryModel.Add(_modelAvailableLaborMarketEEOJobCategory);
                        }
                    }
                    else
                    {
                        foreach (var itemEEOJobCategories in ListEEOJobCategories)
                        {
                            AvailableLaborMarketEEOJobCategoryModel _modelAvailableLaborMarketEEOJobCategory = new AvailableLaborMarketEEOJobCategoryModel();
                            var listRace = _AvailableLaborMarketFileVersions.AvailableLaborMarketDatas.Where(e => e.EEOJobCategory.EEOJobCategoryId == itemEEOJobCategories.EEOJobCategoryId).ToList();
                            if (listRace.Count() > 0)
                            {
                                _modelAvailableLaborMarketEEOJobCategory.EEOJobCategoryId = itemEEOJobCategories.EEOJobCategoryId;
                                _modelAvailableLaborMarketEEOJobCategory.EEOJobCategoryName = itemEEOJobCategories.Name;
                                _modelAvailableLaborMarketEEOJobCategory.EEOJobCategoryNumber = itemEEOJobCategories.EEOJobCategoryNumber;

                                List<AvailableLaborMarketDataModel> _listAvailableLaborMarketDataModel = new List<AvailableLaborMarketDataModel>();
                                foreach (var _itemRace in listRace)
                                {
                                    AvailableLaborMarketDataModel _modelAvailableLaborMarketData = new AvailableLaborMarketDataModel();
                                    _modelAvailableLaborMarketData.AvailableLaborMarketDataId = _itemRace.AvailableLaborMarketDataId;
                                    _modelAvailableLaborMarketData.EEOJobCategoryId = itemEEOJobCategories.EEOJobCategoryId;
                                    _modelAvailableLaborMarketData.RaceId = _itemRace.Race.RaceId;
                                    _modelAvailableLaborMarketData.RaceName = _itemRace.Race.Name;
                                    _modelAvailableLaborMarketData.MaleValue = _itemRace.MaleValue;
                                    _modelAvailableLaborMarketData.FemaleValue = _itemRace.FemaleValue;

                                    _listAvailableLaborMarketDataModel.Add(_modelAvailableLaborMarketData);
                                }
                                _modelAvailableLaborMarketEEOJobCategory.ListAvailableLaborMarketData = _listAvailableLaborMarketDataModel;
                            }

                            _ListAvailableLaborMarketEEOJobCategoryModel.Add(_modelAvailableLaborMarketEEOJobCategory);
                        }
                    }
                    _result.ListAvailableLaborMarketEEOJobCategory = _ListAvailableLaborMarketEEOJobCategoryModel;
                }
                return _result;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetAvailableLaborMarketDataViaFileVersion", "AvailableLaborMarketService.cs");
                throw;
            }
        }
        public ResponseModel SaveAvailableLaborMarketData(AvailableLaborMarketFileVersionModel _AvailableLaborMarketFileVersionModel)
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                Organization organisation = new Organization();
                var users = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();

                if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    Int32 adminselectedOrgId = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    organisation = _context.Organizations.Where(e => e.OrganizationId == adminselectedOrgId).Select(e => e).FirstOrDefault();
                    //_AvailableLaborMarketFileVersions = _context.AvailableLaborMarketFileVersions.Where(e => e.Active == true && e.Organization.OrganizationId == adminselectedOrgId).ToList();
                }
                else
                {
                    if (users != null)
                    {
                        organisation = _context.Users.Where(e => e.UserId == _user).Select(e => e.UserRole.Organization).FirstOrDefault();
                    }
                }


                var _ALMFileVersionsViaOrganisation = _context.AvailableLaborMarketFileVersions.Where(e => e.Organization.OrganizationId == organisation.OrganizationId).ToList();

                var _ALMFileVersions = _ALMFileVersionsViaOrganisation.Where(e => e.AvailableLaborMarketFileVersionId == _AvailableLaborMarketFileVersionModel.AvailableLaborMarketFileVersionId).FirstOrDefault();
                if (_ALMFileVersions != null)
                {
                    _ALMFileVersions.SubmissionDateTime = DateTime.Now;
                    var ALMDates = _ALMFileVersions.AvailableLaborMarketDatas;
                    foreach (var itemEEJob in _AvailableLaborMarketFileVersionModel.ListAvailableLaborMarketEEOJobCategory)
                    {
                        foreach (var itemMarketValue in itemEEJob.ListAvailableLaborMarketData)
                        {
                            AvailableLaborMarketData _Dbmarketdata = null;
                            if (ALMDates != null)
                            {
                                _Dbmarketdata = ALMDates.Where(e => e.EEOJobCategory.EEOJobCategoryId == itemMarketValue.EEOJobCategoryId && e.Race.RaceId == itemMarketValue.RaceId).FirstOrDefault();
                            }
                            if (_Dbmarketdata != null)
                            {
                                _Dbmarketdata.FemaleValue = itemMarketValue.FemaleValue;
                                _Dbmarketdata.MaleValue = itemMarketValue.MaleValue;

                            }
                            else
                            {
                                AvailableLaborMarketData insertAvailableLaborMarketData = new AvailableLaborMarketData
                                {
                                    AvailableLaborMarketFileVersion = _ALMFileVersions,
                                    EEOJobCategory = _context.EEOJobCategories.Where(e => e.EEOJobCategoryId == itemMarketValue.EEOJobCategoryId).FirstOrDefault(),
                                    Race = _context.Races.Where(e => e.RaceId == itemMarketValue.RaceId).FirstOrDefault(),
                                    FemaleValue = itemMarketValue.FemaleValue,
                                    MaleValue = itemMarketValue.MaleValue,
                                    CreateUserId = _user,
                                    CreateDateTime = DateTime.Now,
                                    UpdateUserId = _user,
                                    UpdateDateTime = DateTime.Now
                                };
                                _context.AvailableLaborMarketDatas.Add(insertAvailableLaborMarketData);
                            }
                        }
                    }
                    _context.SaveChanges();
                    return new ResponseModel { Message = "Data updated Successfully", Succeeded = true, Id = 2 };
                }
                else
                {
                    if (_ALMFileVersionsViaOrganisation.Count() > 0)
                    {
                        var ActiveFileVersion = _ALMFileVersionsViaOrganisation.Where(e => e.Active == true).FirstOrDefault();
                        if (ActiveFileVersion != null)
                        {
                            ActiveFileVersion.Active = false;
                        }
                    }
                    List<AvailableLaborMarketData> _lstMarketDate = new List<AvailableLaborMarketData>();
                    foreach (var itemEEJob in _AvailableLaborMarketFileVersionModel.ListAvailableLaborMarketEEOJobCategory)
                    {
                        foreach (var itemMarketValue in itemEEJob.ListAvailableLaborMarketData)
                        {
                            AvailableLaborMarketData insertAvailableLaborMarketData = new AvailableLaborMarketData
                            {
                                AvailableLaborMarketFileVersion = _ALMFileVersions,
                                EEOJobCategory = _context.EEOJobCategories.Where(e => e.EEOJobCategoryId == itemMarketValue.EEOJobCategoryId).FirstOrDefault(),
                                Race = _context.Races.Where(e => e.RaceId == itemMarketValue.RaceId).FirstOrDefault(),

                                FemaleValue = itemMarketValue.FemaleValue,
                                MaleValue = itemMarketValue.MaleValue,
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            _lstMarketDate.Add(insertAvailableLaborMarketData);
                        }
                    }
                    AvailableLaborMarketFileVersion insertAvailableLaborMarketFileVersions = new AvailableLaborMarketFileVersion
                    {
                        Organization = organisation,
                        SubmissionDateTime = DateTime.Now,
                        FileVersionNumber = _ALMFileVersionsViaOrganisation.Count() + 1,
                        Notes = "New",
                        Active = true,
                        CreateUserId = _user,
                        AvailableLaborMarketDatas = _lstMarketDate,
                        CreateDateTime = DateTime.Now,
                        UpdateUserId = _user,
                        UpdateDateTime = DateTime.Now
                    };
                    _context.AvailableLaborMarketFileVersions.Add(insertAvailableLaborMarketFileVersions);
                    _context.SaveChanges();
                    return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = 1 };
                }
                //save in database


            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "SaveAvailableLaborMarketData", "AvailableLaborMarketService.cs");
                throw;
            }
        }
        public String PDFAvailableLaborMarketData(AvailableLaborMarketFileVersionModel _AvailableLaborMarketFileVersion)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<html>");
                sb.Append("<body style='font-family: Arial, Helvetica Neue, Helvetica, sans-serif; font-style: normal; font-variant: normal; font-weight: 500; line-height: normal;  padding: 20px;'>");
                sb.Append("<header style='width:100%'  class='clearfix'>");
                sb.Append("<h2><a><span style='color: #ff7426;'> EEO Now </a>Application</span></h2>");
                sb.Append("</header>");
                sb.Append("<header style='width:100%' class='clearfix'>");
                sb.Append("<h3>AVAILABLE LABOR MARKET DATA (" + DateTime.Now + ")</h3>(File version Number: " + _AvailableLaborMarketFileVersion.FileVersionNumber + ")");
                sb.Append("</header>");
                foreach (var _AlmEEOJobCategory in _AvailableLaborMarketFileVersion.ListAvailableLaborMarketEEOJobCategory.OrderBy(e => e.EEOJobCategoryNumber))
                {
                    sb.Append("<header class='clearfix'>");
                    sb.Append("<h3>" + _AlmEEOJobCategory.EEOJobCategoryNumber.ToString() + " - " + _AlmEEOJobCategory.EEOJobCategoryName + "</h3>");
                    sb.Append("</header>");
                    sb.Append("<table style='width: 100%;' cellspacing='5' cellpadding='2'");
                    sb.Append("<tr>");
                    sb.Append("<td width='60%'></td>");
                    sb.Append("<td><b>Male</b></td>");
                    sb.Append("<td><b>Female</b></td>");
                    sb.Append("</tr>");
                    foreach (var _AvailableLaborMarketData in _AlmEEOJobCategory.ListAvailableLaborMarketData)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td width='60%'><ul><li>" + _AvailableLaborMarketData.RaceName + "</ul></li></td>");
                        sb.Append("<td>" + _AvailableLaborMarketData.MaleValue.Value.ToString("#,##0") + "</td>");
                        sb.Append("<td>" + _AvailableLaborMarketData.FemaleValue.Value.ToString("#,##0") + "</td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");

                }
                sb.Append("</body>");
                sb.Append("</html>");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "PDFAvailableLaborMarketData", "AvailableLaborMarketService.cs");
                throw;
            }
        }

        public List<SelectListItem> GetFileVersionMarketData()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                List<AvailableLaborMarketFileVersion> _AvailableLaborMarketFileVersions = new List<AvailableLaborMarketFileVersion>();
                var users = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                Int32 _selectedOrganizationID = 0;

                if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    _selectedOrganizationID = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _AvailableLaborMarketFileVersions = _context.AvailableLaborMarketFileVersions.Where(e => e.Organization.OrganizationId == _selectedOrganizationID).ToList();
                }
                else
                {
                    if (users != null)
                    {
                        _AvailableLaborMarketFileVersions = users.UserRole.Organization.AvailableLaborMarketFileVersions.ToList();
                    }
                }


                List<SelectListItem> _listLaborMarketFileVersion = new List<SelectListItem>();
                if (_AvailableLaborMarketFileVersions != null)
                {
                    _listLaborMarketFileVersion.AddRange(_AvailableLaborMarketFileVersions.OrderByDescending(e => e.Active).Select(g => new SelectListItem { Text = "Version :" + g.FileVersionNumber.ToString() + "(" + g.SubmissionDateTime.ToString("MM/dd/yyyy") + ")" + "(" + (g.Active == true ? "Current)" : "Previous)"), Value = g.AvailableLaborMarketFileVersionId.ToString() }).ToList());
                }
                return _listLaborMarketFileVersion;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetFileVersionMarketData", "AvailableLaborMarketService.cs");
                throw;
            }
        }

        public void MarkasCurrentLaborMarketData(int AvailableLaborMarketFileVersionId)
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                var users = _context.Users.Where(e => e.UserId == _user).FirstOrDefault();
                List<AvailableLaborMarketFileVersion> _AvailableLaborMarketFileVersions = new List<AvailableLaborMarketFileVersion>();
                if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    Int32 adminselectedOrgId = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _AvailableLaborMarketFileVersions = _context.AvailableLaborMarketFileVersions.Where(e => e.Organization.CreateUserId == adminselectedOrgId).ToList();
                }
                else
                {
                    if (users != null)
                    {
                        _AvailableLaborMarketFileVersions = users.UserRole.Organization.AvailableLaborMarketFileVersions.ToList();
                    }
                }

                foreach (var item in _AvailableLaborMarketFileVersions)
                {
                    if (item.AvailableLaborMarketFileVersionId == AvailableLaborMarketFileVersionId)
                    {
                        item.Active = true;
                    }
                    else { item.Active = false; }
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "MarkasCurrentLaborMarketData", "AvailableLaborMarketService.cs");
                throw;
            }
        }
    }
}
