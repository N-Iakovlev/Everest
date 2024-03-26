using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Incoding.Core.CQRS.Core;


namespace Everest.API;

//public class GeProductForSelectQuery : QueryBase<List<DropDownItemModel>>
  //  {
  //      public string Product { get; set; }
  //
  //      public List<int>? SelectedIds { get; set; }
  //  protected override List<DropDownItemModel> ExecuteResult()
  //  {
  //      SelectedIds ??= new List<int>();
  //      return Repository.Query(new Share.Where.ByFacultyThoughDepartment<Teacher>(FacultyId))
  //          .Select(s => new DropDownItem(s.Id, s.Name, SelectedIds.Contains(s.Id), s.Department.Code, ""))
  //          .ToList();
  //  }
  //  }

