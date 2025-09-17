using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ST10091324_PROG7312_Part1.Models;

namespace ST10091324_PROG7312_Part1.Infrastructure
{
    /// <summary>
    /// Provides comprehensive data validation and integrity checking services
    /// </summary>
    public class DataValidationService
    {
        private readonly Dictionary<Type, List<IDataValidator>> _validators;
        private readonly List<ValidationRule> _customRules;
        
        public DataValidationService()
        {
            _validators = new Dictionary<Type, List<IDataValidator>>();
            _customRules = new List<ValidationRule>();
            
            InitializeDefaultValidators();
        }
        
        /// <summary>
        /// Validates an entity and returns validation results
        /// </summary>
        public async Task<ValidationResult> ValidateAsync<T>(T entity) where T : class
        {
            var result = new ValidationResult
            {
                EntityType = typeof(T).Name,
                Timestamp = DateTime.UtcNow,
                IsValid = true
            };
            
            if (entity == null)
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    PropertyName = "Entity",
                    ErrorMessage = "Entity cannot be null",
                    Severity = ValidationSeverity.Critical
                });
                return result;
            }
            
            // Run built-in data annotations validation
            await ValidateDataAnnotations(entity, result);
            
            // Run custom validators for this type
            await ValidateWithCustomValidators(entity, result);
            
            // Run business rule validation
            await ValidateBusinessRules(entity, result);
            
            // Check referential integrity
            await ValidateReferentialIntegrity(entity, result);
            
            return result;
        }
        
        /// <summary>
        /// Validates multiple entities in batch
        /// </summary>
        public async Task<BatchValidationResult> ValidateBatchAsync<T>(IEnumerable<T> entities) where T : class
        {
            var batchResult = new BatchValidationResult
            {
                Timestamp = DateTime.UtcNow,
                TotalEntities = entities.Count()
            };
            
            var validationTasks = entities.Select(async entity =>
            {
                var result = await ValidateAsync(entity);
                if (result.IsValid)
                    batchResult.ValidEntities++;
                else
                    batchResult.InvalidEntities++;
                    
                return result;
            });
            
            batchResult.Results = await Task.WhenAll(validationTasks);
            batchResult.IsValid = batchResult.InvalidEntities == 0;
            
            return batchResult;
        }
        
        /// <summary>
        /// Registers a custom validator for a specific type
        /// </summary>
        public void RegisterValidator<T>(IDataValidator<T> validator) where T : class
        {
            var type = typeof(T);
            if (!_validators.ContainsKey(type))
            {
                _validators[type] = new List<IDataValidator>();
            }
            
            _validators[type].Add(validator);
        }
        
        /// <summary>
        /// Adds a custom validation rule
        /// </summary>
        public void AddValidationRule(ValidationRule rule)
        {
            _customRules.Add(rule);
        }
        
        /// <summary>
        /// Validates using data annotations
        /// </summary>
        private async Task ValidateDataAnnotations<T>(T entity, ValidationResult result)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(entity);
            var annotationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            
            var isValid = Validator.TryValidateObject(entity, validationContext, annotationResults, true);
            
            if (!isValid)
            {
                result.IsValid = false;
                foreach (var annotationResult in annotationResults)
                {
                    result.Errors.Add(new ValidationError
                    {
                        PropertyName = annotationResult.MemberNames.FirstOrDefault() ?? "Unknown",
                        ErrorMessage = annotationResult.ErrorMessage,
                        Severity = ValidationSeverity.Error
                    });
                }
            }
        }
        
        /// <summary>
        /// Validates using custom validators
        /// </summary>
        private async Task ValidateWithCustomValidators<T>(T entity, ValidationResult result)
        {
            var entityType = typeof(T);
            if (_validators.TryGetValue(entityType, out var validators))
            {
                foreach (var validator in validators)
                {
                    if (validator is IDataValidator<T> typedValidator)
                    {
                        var validatorResult = await typedValidator.ValidateAsync(entity);
                        if (!validatorResult.IsValid)
                        {
                            result.IsValid = false;
                            result.Errors.AddRange(validatorResult.Errors);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Validates business rules
        /// </summary>
        private async Task ValidateBusinessRules<T>(T entity, ValidationResult result)
        {
            var entityType = typeof(T);
            var applicableRules = _customRules.Where(r => r.AppliesTo == entityType || r.AppliesTo == null);
            
            foreach (var rule in applicableRules)
            {
                try
                {
                    var ruleResult = await rule.ValidateAsync(entity);
                    if (!ruleResult.IsValid)
                    {
                        result.IsValid = false;
                        result.Errors.AddRange(ruleResult.Errors);
                    }
                }
                catch (Exception ex)
                {
                    result.Warnings.Add(new ValidationWarning
                    {
                        Message = $"Business rule validation failed: {ex.Message}",
                        RuleName = rule.Name
                    });
                }
            }
        }
        
        /// <summary>
        /// Validates referential integrity
        /// </summary>
        private async Task ValidateReferentialIntegrity<T>(T entity, ValidationResult result)
        {
            // Check for common referential integrity issues
            var properties = typeof(T).GetProperties();
            
            foreach (var property in properties)
            {
                // Check for foreign key properties
                if (property.Name.EndsWith("Id") && property.PropertyType == typeof(int))
                {
                    var value = (int)property.GetValue(entity);
                    if (value < 0)
                    {
                        result.Errors.Add(new ValidationError
                        {
                            PropertyName = property.Name,
                            ErrorMessage = $"Foreign key {property.Name} cannot be negative",
                            Severity = ValidationSeverity.Error
                        });
                        result.IsValid = false;
                    }
                }
                
                // Check for required reference properties
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    var requiredAttribute = property.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();
                    if (requiredAttribute != null && property.GetValue(entity) == null)
                    {
                        result.Errors.Add(new ValidationError
                        {
                            PropertyName = property.Name,
                            ErrorMessage = $"Required reference {property.Name} is null",
                            Severity = ValidationSeverity.Error
                        });
                        result.IsValid = false;
                    }
                }
            }
        }
        
        /// <summary>
        /// Initializes default validators for common entity types
        /// </summary>
        private void InitializeDefaultValidators()
        {
            // User validator
            RegisterValidator(new UserValidator());
            
            // Service Request validator
            RegisterValidator(new ServiceRequestValidator());
            
            // Local Event validator
            RegisterValidator(new LocalEventValidator());
            
            // Add common business rules
            AddValidationRule(new EmailValidationRule());
            AddValidationRule(new PhoneNumberValidationRule());
            AddValidationRule(new DateRangeValidationRule());
        }
    }
    
    /// <summary>
    /// Interface for data validators
    /// </summary>
    public interface IDataValidator
    {
        Task<ValidationResult> ValidateAsync(object entity);
    }
    
    /// <summary>
    /// Generic interface for typed data validators
    /// </summary>
    public interface IDataValidator<T> : IDataValidator where T : class
    {
        Task<ValidationResult> ValidateAsync(T entity);
    }
    
    /// <summary>
    /// Base class for data validators
    /// </summary>
    public abstract class DataValidator<T> : IDataValidator<T> where T : class
    {
        public abstract Task<ValidationResult> ValidateAsync(T entity);
        
        public async Task<ValidationResult> ValidateAsync(object entity)
        {
            if (entity is T typedEntity)
            {
                return await ValidateAsync(typedEntity);
            }
            
            return new ValidationResult
            {
                IsValid = false,
                Errors = new List<ValidationError>
                {
                    new ValidationError
                    {
                        PropertyName = "Entity",
                        ErrorMessage = $"Expected {typeof(T).Name} but received {entity?.GetType().Name ?? "null"}",
                        Severity = ValidationSeverity.Critical
                    }
                }
            };
        }
    }
    
    /// <summary>
    /// User entity validator
    /// </summary>
    public class UserValidator : DataValidator<User>
    {
        public override async Task<ValidationResult> ValidateAsync(User user)
        {
            var result = new ValidationResult { IsValid = true };
            
            // Validate email format
            if (!string.IsNullOrEmpty(user.Email) && !IsValidEmail(user.Email))
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    PropertyName = nameof(user.Email),
                    ErrorMessage = "Invalid email format",
                    Severity = ValidationSeverity.Error
                });
            }
            
            // Validate username uniqueness (would require database check in real implementation)
            if (string.IsNullOrWhiteSpace(user.Username))
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    PropertyName = nameof(user.Username),
                    ErrorMessage = "Username is required",
                    Severity = ValidationSeverity.Error
                });
            }
            
            return result;
        }
        
        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
        }
    }
    
    /// <summary>
    /// Service Request entity validator
    /// </summary>
    public class ServiceRequestValidator : DataValidator<ServiceRequest>
    {
        public override async Task<ValidationResult> ValidateAsync(ServiceRequest serviceRequest)
        {
            var result = new ValidationResult { IsValid = true };
            
            // Validate priority range
            if (serviceRequest.Priority < 1 || serviceRequest.Priority > 5)
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    PropertyName = nameof(serviceRequest.Priority),
                    ErrorMessage = "Priority must be between 1 and 5",
                    Severity = ValidationSeverity.Error
                });
            }
            
            // Validate status
            var validStatuses = new[] { "Open", "In Progress", "Resolved", "Closed" };
            if (!validStatuses.Contains(serviceRequest.Status))
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    PropertyName = nameof(serviceRequest.Status),
                    ErrorMessage = "Invalid status value",
                    Severity = ValidationSeverity.Error
                });
            }
            
            // Validate date logic
            if (serviceRequest.CreatedDate > DateTime.Now)
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    PropertyName = nameof(serviceRequest.CreatedDate),
                    ErrorMessage = "Created date cannot be in the future",
                    Severity = ValidationSeverity.Error
                });
            }
            
            return result;
        }
    }
    
    /// <summary>
    /// Local Event entity validator
    /// </summary>
    public class LocalEventValidator : DataValidator<LocalEvent>
    {
        public override async Task<ValidationResult> ValidateAsync(LocalEvent localEvent)
        {
            var result = new ValidationResult { IsValid = true };
            
            // Validate date range
            if (localEvent.StartDate >= localEvent.EndDate)
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    PropertyName = nameof(localEvent.StartDate),
                    ErrorMessage = "Start date must be before end date",
                    Severity = ValidationSeverity.Error
                });
            }
            
            // Validate category
            var validCategories = new[] { "Community", "Government", "Emergency", "Cultural", "Educational" };
            if (!validCategories.Contains(localEvent.Category))
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    PropertyName = nameof(localEvent.Category),
                    ErrorMessage = "Invalid category",
                    Severity = ValidationSeverity.Error
                });
            }
            
            return result;
        }
    }
    
    /// <summary>
    /// Validation rule for custom business logic
    /// </summary>
    public class ValidationRule
    {
        public string Name { get; set; }
        public Type AppliesTo { get; set; }
        public Func<object, Task<ValidationResult>> ValidateAsync { get; set; }
    }
    
    /// <summary>
    /// Email validation rule
    /// </summary>
    public class EmailValidationRule : ValidationRule
    {
        public EmailValidationRule()
        {
            Name = "EmailValidation";
            ValidateAsync = async (entity) =>
            {
                var result = new ValidationResult { IsValid = true };
                var properties = entity.GetType().GetProperties()
                    .Where(p => p.Name.ToLower().Contains("email") && p.PropertyType == typeof(string));
                
                foreach (var property in properties)
                {
                    var email = property.GetValue(entity) as string;
                    if (!string.IsNullOrEmpty(email) && !IsValidEmail(email))
                    {
                        result.IsValid = false;
                        result.Errors.Add(new ValidationError
                        {
                            PropertyName = property.Name,
                            ErrorMessage = "Invalid email format",
                            Severity = ValidationSeverity.Error
                        });
                    }
                }
                
                return result;
            };
        }
        
        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
        }
    }
    
    /// <summary>
    /// Phone number validation rule
    /// </summary>
    public class PhoneNumberValidationRule : ValidationRule
    {
        public PhoneNumberValidationRule()
        {
            Name = "PhoneNumberValidation";
            ValidateAsync = async (entity) =>
            {
                var result = new ValidationResult { IsValid = true };
                var properties = entity.GetType().GetProperties()
                    .Where(p => p.Name.ToLower().Contains("phone") && p.PropertyType == typeof(string));
                
                foreach (var property in properties)
                {
                    var phone = property.GetValue(entity) as string;
                    if (!string.IsNullOrEmpty(phone) && !IsValidPhoneNumber(phone))
                    {
                        result.IsValid = false;
                        result.Errors.Add(new ValidationError
                        {
                            PropertyName = property.Name,
                            ErrorMessage = "Invalid phone number format",
                            Severity = ValidationSeverity.Warning
                        });
                    }
                }
                
                return result;
            };
        }
        
        private bool IsValidPhoneNumber(string phone)
        {
            // Simple phone number validation - can be enhanced
            var phoneRegex = new Regex(@"^[\+]?[1-9][\d\s\-\(\)]{7,15}$");
            return phoneRegex.IsMatch(phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", ""));
        }
    }
    
    /// <summary>
    /// Date range validation rule
    /// </summary>
    public class DateRangeValidationRule : ValidationRule
    {
        public DateRangeValidationRule()
        {
            Name = "DateRangeValidation";
            ValidateAsync = async (entity) =>
            {
                var result = new ValidationResult { IsValid = true };
                var properties = entity.GetType().GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));
                
                foreach (var property in properties)
                {
                    var dateValue = property.GetValue(entity);
                    if (dateValue is DateTime date)
                    {
                        // Check for reasonable date ranges
                        if (date < new DateTime(1900, 1, 1) || date > DateTime.Now.AddYears(100))
                        {
                            result.IsValid = false;
                            result.Errors.Add(new ValidationError
                            {
                                PropertyName = property.Name,
                                ErrorMessage = "Date is outside reasonable range",
                                Severity = ValidationSeverity.Warning
                            });
                        }
                    }
                }
                
                return result;
            };
        }
    }
    
    /// <summary>
    /// Validation result containing errors and warnings
    /// </summary>
    public class ValidationResult
    {
        public string EntityType { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsValid { get; set; }
        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();
        public List<ValidationWarning> Warnings { get; set; } = new List<ValidationWarning>();
    }
    
    /// <summary>
    /// Batch validation result for multiple entities
    /// </summary>
    public class BatchValidationResult
    {
        public DateTime Timestamp { get; set; }
        public bool IsValid { get; set; }
        public int TotalEntities { get; set; }
        public int ValidEntities { get; set; }
        public int InvalidEntities { get; set; }
        public ValidationResult[] Results { get; set; }
    }
    
    /// <summary>
    /// Validation error details
    /// </summary>
    public class ValidationError
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public ValidationSeverity Severity { get; set; }
        public object AttemptedValue { get; set; }
    }
    
    /// <summary>
    /// Validation warning details
    /// </summary>
    public class ValidationWarning
    {
        public string Message { get; set; }
        public string RuleName { get; set; }
    }
    
    /// <summary>
    /// Validation severity levels
    /// </summary>
    public enum ValidationSeverity
    {
        Info,
        Warning,
        Error,
        Critical
    }
}