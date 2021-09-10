using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SectionMappers
    {
       public static Section MapToSection (this Sectionv3 a)
        {
            return new Section
            {
                AvailableCreditConversion = a.AvailableCreditConversion,
                AvailableCredits = a.AvailableCredits,
                AvailableCreditType = a.AvailableCreditType,
                Characteristics = a.Characteristics?.Select(x => x.MapToSectionCharacteristicsItem()).ToList(),
                ClassPeriodReference = null,
                CourseOfferingReference = a.CourseOfferingReference?.MapToSectionCourseOfferingReference(),
                EducationalEnvironmentType = a.EducationalEnvironmentType,
                Id = a.Id,
                InstructionLanguageDescriptor = a.InstructionLanguageDescriptor,
                LocationReference = a.LocationReference?.MapToSectionLocationReference(),
                MediumOfInstructionType = a.MediumOfInstructionType,
                PopulationServedType = a.PopulationServedType,
                Programs = a.Programs?.Select(x => x.MapToSectionProgramsItem()).ToList(),
                SchoolReference = null,
                SequenceOfCourse = a.SequenceOfCourse,
                UniqueSectionCode = a.UniqueSectionCode,
                _etag = a._etag
            };
        }

        public static Sectionv3 MapToSection(this Section a)
        {
            return new Sectionv3
            {
                AvailableCreditConversion = a.AvailableCreditConversion,
                AvailableCredits = a.AvailableCredits,
                AvailableCreditType = a.AvailableCreditType,
                Characteristics = a.Characteristics?.Select(x => x.MapToSectionCharacteristicsItem()).ToList(),
                CourseOfferingReference = a.CourseOfferingReference?.MapToSectionCourseOfferingReference(),
                EducationalEnvironmentType = a.EducationalEnvironmentType,
                Id = a.Id,
                InstructionLanguageDescriptor = a.InstructionLanguageDescriptor,
                LocationReference = a.LocationReference?.MapToSectionLocationReference(),
                MediumOfInstructionType = a.MediumOfInstructionType,
                PopulationServedType = a.PopulationServedType,
                Programs = a.Programs?.Select(x => x.MapToSectionProgramsItem()).ToList(),
                SequenceOfCourse = a.SequenceOfCourse,
                UniqueSectionCode = a.UniqueSectionCode,
                _etag = a._etag,
                ClassPeriods = null,
                CourseLevelCharacteristics = null,
                LocationSchoolReference = null,
                OfferedGradeLevels = null,
                OfficialAttendancePeriod = false,
                SectionName = null,
            };
        }

        public static Section MapToSection(this EnrollmentSectionv3 a)
        {
            SectionClassPeriodReference getClassPeriod()
            {
                if (a.ClassPeriods == null)
                    return null;

                if (a.ClassPeriods.Count == 0)
                    return null;

                var classPeriods = a.ClassPeriods.First();

                return new SectionClassPeriodReference
                {
                    Link = classPeriods.ClassPeriodReference?.Link?.MapToSectionClassPeriodReferenceLink(),
                    Name = classPeriods.ClassPeriodName,
                    SchoolId = (classPeriods.ClassPeriodReference?.SchoolId) ?? 0,
                };
            }

            SectionCourseOfferingReference getSectionCourseOfferingReference()
            {
                if (a.Session == null)
                    return null;

                return new SectionCourseOfferingReference
                {
                    LocalCourseCode = a.LocalCourseCode,
                    SchoolId = a.Session.SchoolId,
                    SchoolYear = a.Session.SchoolYear,
                    TermDescriptor = a.Session.TermDescriptor?.MapToTermDescriptorv2(),
                };
            }

            SectionSchoolReference getSectionSchoolReference()
            {
                if (a.Session == null)
                    return null;

                return new SectionSchoolReference
                {
                    SchoolId = a.Session.SchoolId,
                };
            }

            return new Section
            {
                AvailableCreditConversion = null, // TODO Check
                AvailableCredits = a.AvailableCredits,
                AvailableCreditType = null, // TODO Check
                Characteristics = null, // TODO Check
                ClassPeriodReference = getClassPeriod(),
                CourseOfferingReference = getSectionCourseOfferingReference(),
                EducationalEnvironmentType = a.EducationalEnvironmentDescriptor?.MapToEducationalEnvironmentDescriptorv2(),
                Id = a.Id,
                InstructionLanguageDescriptor = null, // TODO Check
                LocationReference = null,  // TODO Check
                MediumOfInstructionType = null, // TODO Check
                PopulationServedType = null, // TODO Check
                Programs = null, // TODO Check
                SchoolReference = getSectionSchoolReference(),
                SequenceOfCourse = a.SequenceOfCourse,
                UniqueSectionCode = a.SectionIdentifier,
                _etag = a._etag,
            };
        }
    }
}
