using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.DTOs.Interventions;
using EdFi.RtI.Core.DTOs.StaffEducationOrganizationAssignmentAssociations;
using EdFi.RtI.Core.DTOs.Staffs;
using EdFi.RtI.Core.DTOs.Student;
using EdFi.RtI.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public class EdFiMapperV2 : IEdFiMapper
    {
        public T Map<T>(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public T Map<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public U Map<T, U>(T obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<U>(json);
        }

        public StaffDTO MapStaffDTO(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            var staffDTO = JsonConvert.DeserializeObject<StaffDTO>(json);

            staffDTO = SetFullName(staffDTO);

            return staffDTO;
        }

        public IEnumerable<StaffDTO> MapStaffDTOAll(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            var staffDTOs = JsonConvert.DeserializeObject<IEnumerable<StaffDTO>>(json);

            return staffDTOs
                .Select(staffDTO =>
                {
                    staffDTO = SetFullName(staffDTO);

                    return staffDTO;
                })
                .ToList();
        }

        public StaffEducationOrganizationAssignmentAssociationDTO MapStaffEducationOrganizationAssignmentAssociationDTO(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<StaffEducationOrganizationAssignmentAssociationDTO>(json);
        }

        public IEnumerable<StaffEducationOrganizationAssignmentAssociationDTO> MapStaffEducationOrganizationAssignmentAssociationDTOAll(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<IEnumerable<StaffEducationOrganizationAssignmentAssociationDTO>>(json);
        }

        /// <summary>
        /// DEPRECATED. Build UserSessionProfileDTO manually through class properties instead
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="association"></param>
        /// <returns></returns>
        public UserSessionProfile MapUserSessionProfile(StaffDTO staff, StaffEducationOrganizationAssignmentAssociation association)
        {
            return new UserSessionProfile()
            {
                Staff = staff,
                StaffEducationOrganizationAssignmentAssociation = association,
            };
        }

        private StaffDTO SetFullName(StaffDTO staff)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(staff.FirstName))
                sb.Append(staff.FirstName).Append(" ");

            if (!string.IsNullOrWhiteSpace(staff.MiddleName))
                sb.Append(staff.MiddleName).Append(" ");

            if (!string.IsNullOrWhiteSpace(staff.LastSurname))
                sb.Append(staff.LastSurname).Append(" ");

            staff.FullName = sb.ToString().Trim();

            return staff;
        }

        public InterventionDTO MapInterventionDTO(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            var interventionDTO = JsonConvert.DeserializeObject<InterventionDTO>(json);
            return interventionDTO;
        }

        public IEnumerable<InterventionDTO> MapInterventionDTOALL(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            var interventionDTO = JsonConvert.DeserializeObject<IEnumerable<InterventionDTO>>(json);
            return interventionDTO;
        }

        public StudentDTO MapStudentDTO(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            var studentDTO = JsonConvert.DeserializeObject<StudentDTO>(json);
            return studentDTO;
        }

        public IEnumerable<StudentDTO> MapStudentDTOALL(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            var studentDTO = JsonConvert.DeserializeObject<IEnumerable<StudentDTO>>(json);
            return studentDTO;
        }

        public StaffDTO Secured(StaffDTO staff)
        {
            staff.IdentificationCodes = staff.IdentificationCodes.Where(obj => obj.StaffIdentificationSystemDescriptor != "SSN").ToList();

            return staff;
        }

        public Staff Secured(Staff staff)
        {
            staff.IdentificationCodes = staff.IdentificationCodes.Where(obj => obj.StaffIdentificationSystemDescriptor != "SSN").ToList();

            return staff;
        }
        
        public Student Secured(Student student)
        {
            student.IdentificationCodes = student.IdentificationCodes.Where(obj => obj.AssigningOrganizationIdentificationCode != "SSN").ToList();
            return student;
        }

        public StudentDTO SecuredStudentDTO(object student)
        {
            var parsedStudentDto = Map<StudentDTO>(student);
            return SecuredStudentDTO(parsedStudentDto);
        }

        public StudentDTO SecuredStudentDTO(Student student)
        {
            var parsedStudentDto = Map<Student, StudentDTO>(student);
            return SecuredStudentDTO(parsedStudentDto);
        }

        public StudentDTO SecuredStudentDTO(StudentDTO student)
        {
            if (student.IdentificationCodes != null)
            {
                student.IdentificationCodes = student.IdentificationCodes
                    .Where(obj => obj.AssigningOrganizationIdentificationCode != "SSN")
                    .ToList();
            }

            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(student.LastSurname))
            {
                sb.Append(student.LastSurname);

                if (!string.IsNullOrWhiteSpace(student.FirstName))
                    sb.Append(", ");
                else if (!string.IsNullOrWhiteSpace(student.MiddleName))
                    sb.Append(", ");
            }

            if (!string.IsNullOrWhiteSpace(student.FirstName))
            {
                sb.Append(student.FirstName);

                if (!string.IsNullOrWhiteSpace(student.MiddleName))
                    sb.Append(" ");
            }

            if (!string.IsNullOrWhiteSpace(student.MiddleName))
            {
                sb.Append(student.MiddleName[0]).Append(".");
            }

            student.FullName = sb.ToString().Trim();

            return student;
        }

        public IEnumerable<StaffDTO> Secured(IEnumerable<StaffDTO> staffs)
        {
            return staffs.Select(staff => Secured(staff)).ToList();
        }

        public IEnumerable<Staff> Secured(IEnumerable<Staff> staffs)
        {
            return staffs.Select(staff => Secured(staff)).ToList();
        }
        
        public IEnumerable<StudentDTO> Secured(IEnumerable<StudentDTO> students)
        {
            return students.Select(student => SecuredStudentDTO(student)).ToList();
        }
        
        public IEnumerable<Student> Secured(IEnumerable<Student> students)
        {
            return students.Select(student => Secured(student)).ToList();
        }
    }
}
