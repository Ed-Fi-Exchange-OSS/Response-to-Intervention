using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.DTOs.Interventions;
using EdFi.RtI.Core.DTOs.StaffEducationOrganizationAssignmentAssociations;
using EdFi.RtI.Core.DTOs.Staffs;
using EdFi.RtI.Core.DTOs.Student;
using EdFi.RtI.Core.DTOs.StudentSectionAssociation;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public interface IEdFiMapper : IDomainService
    {
        T Map<T>(object obj);
        T Map<T>(string json);
        U Map<T, U>(T obj);

        StaffDTO MapStaffDTO(object obj);
        IEnumerable<StaffDTO> MapStaffDTOAll(object obj);
        StaffEducationOrganizationAssignmentAssociationDTO MapStaffEducationOrganizationAssignmentAssociationDTO(object obj);
        IEnumerable<StaffEducationOrganizationAssignmentAssociationDTO> MapStaffEducationOrganizationAssignmentAssociationDTOAll(object obj);
        UserSessionProfile MapUserSessionProfile(StaffDTO staff, StaffEducationOrganizationAssignmentAssociation association);
        InterventionDTO MapInterventionDTO(object obj);
        IEnumerable<InterventionDTO> MapInterventionDTOALL(object obj);

        StudentDTO MapStudentDTO(object obj);
        IEnumerable<StudentDTO> MapStudentDTOALL(object obj);
        
        Staff Secured(Staff staff);
        StaffDTO Secured(StaffDTO staff);
        Student Secured(Student student);

        StudentDTO SecuredStudentDTO(object student);
        StudentDTO SecuredStudentDTO(Student student);
        StudentDTO SecuredStudentDTO(StudentDTO student);

        IEnumerable<StaffDTO> Secured(IEnumerable<StaffDTO> staffs);
        IEnumerable<Staff> Secured(IEnumerable<Staff> staffs);
        IEnumerable<StudentDTO> Secured(IEnumerable<StudentDTO> students);
        IEnumerable<Student> Secured(IEnumerable<Student> students);
    }
}
