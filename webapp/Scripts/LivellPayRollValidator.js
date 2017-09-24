$('.employee_info_section #fuelux-wizard').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        FName: {
            message: 'The first name is not valid',
            validators: {
                notEmpty: {
                    message: 'The first name is required and cannot be empty'
                },
                regexp: {
                    regexp: /^[a-zA-Z0-9_\u4e00-\u9fa5]+$/,
                    message: 'The first name can only consist of alphabetical, number, dot and underscore'
                }
            }
        },
        LName: {
            message: 'The Last Name is not valid',
            validators: {
                notEmpty: {
                    message: 'The last name is required and cannot be empty'
                },
                regexp: {
                    regexp: /^[a-zA-Z0-9_\u4e00-\u9fa5]+$/,
                    message: 'The last name can only consist of alphabetical, number, dot and underscore'
                }
            }
        },
        SSN: {
            message: 'The ssn is not valid',
            validators: {
                notEmpty: {
                    message: 'The SSN is required and cannot be empty'
                },
                regexp: {
                    regexp: /^\d{3}-\d{2}-\d{4}/,
                    message: 'Please input the correct information about SSN! '
                }
            }
        },
        Email: {
            validators: {
                notEmpty: {
                    message: 'The email is required and cannot be empty'
                },
                emailAddress: {
                    message: 'The input is not a valid email address'
                }
            }
        },
        TimeZone: {
            validators: {
                notEmpty: {
                    message: 'The TimeZone is required Please Choose TimeZone'
                }
            }
        },
        Address1: {
            message: 'The address is not valid',
            validators: {
                notEmpty: {
                    message: 'The address is required and cannot be empty'
                },
                stringLength: {
                    min: 1,
                    max: 128,
                    message: 'The address must be more than 1 and less than 128 characters long'
                }
            }
        },
        City: {
            message: 'The City is not valid',
            validators: {
                notEmpty: {
                    message: 'The city is required and cannot be empty'
                }
            }
        },
        State: {
            message: 'The State is not valid',
            validators: {
                notEmpty: {
                    message: 'The city is required Please Choose State'
                }
            }
        },
        ZipCode: {
            message: 'The ZipCode is not valid',
            validators: {
                notEmpty: {
                    message: 'The zipcode is required and cannot be empty'
                },
                numeric: { message: 'The zipcode can only consist of number' }
            }
        },
        Phone: {
            message: 'The Phone is not valid',
            validators: {
                notEmpty: {
                    message: 'The phone is required and cannot be empty'
                },
                regexp: {
                    regexp: /^\(\d{3}\)-\d{3}-\d{4}/,
                    message: 'Please input the correct information about Phone! '
                }
            }
        },
        UserRole: {
            message: 'The role is not valid',
            validators: {
                notEmpty: {
                    message: 'The role is required and cannot be empty'
                }
            }
        },
        PTOAccRate: {
            message: 'The PTO is not valid'
        },
        PTOCapHours: {
            message: 'The paid time off maximum hours is not valid',
            validators: {
                notEmpty: {
                    message: 'The paid time off maximum hours is required and cannot be empty'
                }
            }
        },
        VacAccRate: {
            message: 'The paid vacation is not valid'
        },
        VacCapHours: {
            message: 'The paid vacation maximum hours is not valid',
            validators: {
                notEmpty: {
                    message: 'The paid vacation maximum hours is required and cannot be empty'
                }
            }
        },
        F102: {
            message: 'The paid vacation maximum hours is not valid',
            validators: {
                notEmpty: {
                    message: 'The paid vacation maximum hours is required and cannot be empty'
                }
            }
        },
        F1231: {
            message: 'The paid vacation maximum hours is not valid',
            validators: {
                notEmpty: {
                    message: 'The paid vacation maximum hours is required and cannot be empty'
                }
            }
        }
    }
});

$('#CustomerForm').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        CustomerName: {
            message: 'The customer name is not valid',
            validators: {
                notEmpty: {
                    message: 'The customer name is required and cannot be empty'
                },
                stringLength: {
                    min: 1,
                    max: 128,
                    message: 'The customer name must be more than 1 and less than 128 characters long'
                }
            }
        },
        Telphone: {
            message: 'The telphone is not valid',
            validators: {
                notEmpty: {
                    message: 'The telphone is required and cannot be empty'
                }
            }
        },
        Fax: {
            message: 'The fax is not valid',
            validators: {
                numeric: {
                    message: 'The telphone only allow input telphone number'
                }
            }
        },
        Address: {
            message: 'The fax is not valid',
            validators: {
                stringLength: {
                    min: 1,
                    max: 256,
                    message: 'The address must be more than 1 and less than 256 characters long'
                }
            }
        },
        Remark: {
            message: 'The fax is not valid',
            validators: {
                stringLength: {
                    min: 1,
                    max: 512,
                    message: 'The remark must be more than 1 and less than 512 characters long'
                }
            }
        }
    }
});
//Add Job Item
$('#Job-Add').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        JobName: {
            message: 'The job name is not valid',
            validators: {
                notEmpty: {
                    message: 'The job name is required and cannot be empty.'
                }
            }
        }
    }
});
//_JobForm
$('#JobForm').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        JobName: {
            message: 'The job name is not valid',
            validators: {
                notEmpty: {
                    message: 'The job name is required and cannot be empty.'
                }
            }
        }
    }
});
//TimeSheet-Add
$('#TimeSheet-Add').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        EmployeeId: {
            message: 'The Employee Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The Employee Is Required And Cannot Be Empty.'
                }
            }
        },
        JobId: {
                message: 'The Job Is Not Valid',
                validators: {
                    notEmpty: {
                        message: 'The Job Is Required And Cannot Be Empty.'
                    }
                }
        },
        StartDate: {
            message: 'The StartDate Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The StartDate Is Required And Cannot Be Empty.'
                }
            }
        },
        StartDateTime: {
            message: 'The StartDate Time Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The StartDate Time Is Required And Cannot Be Empty.'
                }
            }
        },
        StopDate: {
            message: 'The StopDate Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The StopDate Is Required And Cannot Be Empty.'
                }
            }
        },
        StopDateTime: {
            message: 'The StopDate Time Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The StopDate Time Is Required And Cannot Be Empty.'
                }
            }
        },
        TimeSheetType: {
            message: 'The TimeSheet Type Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The TimeSheet Type Is Required And Cannot Be Empty.'
                }
            }
        }
    }
});
//TimeSheet-Edit
$('#TimeSheet-Edit').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        StartDate: {
            message: 'The StartDate Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The StartDate Is Required And Cannot Be Empty.'
                }
            }
        },
        StartDateTime: {
            message: 'The StartDate Time Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The StartDate Time Is Required And Cannot Be Empty.'
                }
            }
        },
        StopDate: {
            message: 'The StopDate Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The StopDate Is Required And Cannot Be Empty.'
                }
            }
        },
        StopDateTime: {
            message: 'The StopDate Time Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The StopDate Time Is Required And Cannot Be Empty.'
                }
            }
        },
        TimeSheetType: {
            message: 'The TimeSheet Type Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The TimeSheet Type Is Required And Cannot Be Empty.'
                }
            }
        }
    }
});
//PayRollSetup
$('#PayRollSetup').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        CompanyName: {
            message: 'The Company Name Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The Company Name Required And Cannot Be Empty.'
                }
            }
        },
        FedTaxId: {
            message: 'The Fed Tax Id Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The Fed Tax Id Required And Cannot Be Empty.'
                }
            }
        },
        Address1: {
            message: 'The Address Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The Address Required And Cannot Be Empty.'
                }
            }
        },
        City: {
            message: 'The City Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The City Required And Cannot Be Empty.'
                }
            }
        },
        State: {
            message: 'The State Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The State Required And Cannot Be Empty.'
                }
            }
        },
        Zip: {
            message: 'The Zip Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The Zip Required And Cannot Be Empty.'
                }
            }
        },
        Email: {
            message: 'The Email Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The email required and cnnot be empty.'
                },
                emailAddress: {
                    message: 'The input is not a valid email address.'
                }
            }
        },
        PayFreq: {
            message: 'The Period Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The PayFreq required and cannot be empty.'
                }
            }
        },
        TimeZone: {
            message: 'The Period Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The TimeZone required and cannot be empty.'
                }
            }
        },
        RoundTo: {
            message: 'The Period Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The roundto required and cannot be empty.'
                }
            }
        }
    }
});
//TaxSetup
$('#TaxSetup').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        FedTaxId: {
            message: 'The Tax ID (EIN) Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The tax id (EIN) required and cannot Be empty.'
                }
            }
        }
    }
});
//timecard-event-form
$('#timecard-event-form').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        JobId: {
            message: 'The Job Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The job is required and cannot be empty.'
                }
            }
        }
    }
});
//FormConfirmedEmail
$('#FormConfirmedEmail').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        FName: {
            message: 'The admin first name required and cannot be empty',
            validators: {
                notEmpty: {
                    message: 'The admin first name required and cannot be empty.'
                }
            }
        },
        LName: {
            message: 'The admin last name required and cannot be empty',
            validators: {
                notEmpty: {
                    message: 'The admin last name required and cannot be empty.'
                }
            }
        },
        SSN: {
            message: 'The admin SSN required and cannot be empty',
            validators: {
                notEmpty: {
                    message: 'The admin SSN required and cannot be empty.'
                }
            }
        },
        Telephone: {
            message: 'The admin telephone required and cannot be empty',
            validators: {
                notEmpty: {
                    message: 'The admin telephone required and cannot be empty.'
                }
            }
        },
        Email: {
            message: 'The email is not valid',
            validators: {
                notEmpty: {
                    message: 'The email required and cannot be empty.'
                },
                emailAddress: {
                    message: 'The input is not a valid email address.'
                }
            }
        },
        Password: {
            message: 'The password is not valid',
            validators: {
                notEmpty: {
                    message: 'The password required and cannot be empty.'
                }
            }
        },
        PasswordConfirm: {
            message: 'The password is not valid',
            validators: {
                notEmpty: {
                    message: 'The password required and cannot be empty.'
                },
                identical: {
                    field: 'Password',
                    message: 'Please enter the same password as above'
                },
            }
        }
    }
});
//FormResetPassword
$('#FormResetPassword').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        Password: {
            message: 'The password is not valid',
            validators: {
                notEmpty: {
                    message: 'The password required and cannot be empty.'
                }
            }
        },
        PasswordConfirm: {
            message: 'The password is not valid',
            validators: {
                notEmpty: {
                    message: 'The password required and cannot be empty.'
                },
                identical: {
                    field: 'Password',
                    message: 'Please enter the same password as above'
                },
            }
        }
    }
});
//GeneralSetForm
$('#GeneralSetForm').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        Email: {
            message: 'The Account Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The account cannot be empty.'
                },
                emailAddress: {
                    message: 'The input is not a valid email address'
                }
            }
        },
        PayRollUser: {
            message: 'The Nickname Is Not Valid',
            validators: {
                notEmpty: {
                    message: 'The nickname is required and cannot be empty.'
                }
            }
        },
        TimeZone: {
            validators: {
                notEmpty: {
                    message: 'The city is required, Please choose TimeZone'
                }
            }
        }
    }
});

//ManagerAddForm1
$('#ManagerAddForm').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        UserName: {
            message: 'The manager nmae is not valid',
            validators: {
                notEmpty: {
                    message: 'The manager name required and cannot Be empty.'
                }
            }
        },
        Email: {
            message: 'The email is not valid',
            validators: {
                notEmpty: {
                    message: 'The email required and cannot Be empty.'
                }
            }
        }
    }
});

//FormUserConfirmed
$('#FormUserConfirmed').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        Password: {
            message: 'The password is not valid',
            validators: {
                notEmpty: {
                    message: 'The password required and cannot be empty.'
                }
            }
        },
        PasswordConfirm: {
            message: 'The password is not valid',
            validators: {
                notEmpty: {
                    message: 'The password required and cannot be empty.'
                },
                identical: {
                    field: 'Password',
                    message: 'Please enter the same password as above'
                },
            }
        }
    }
});
//PTO-Add
$('#PTO-Add').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        EmployeeId: {
            message: 'The employee is not valid',
            validators: {
                notEmpty: {
                    message: 'The employee required and cannot be empty.'
                }
            }
        },
        Date: {
            message: 'The date is not valid',
            validators: {
                notEmpty: {
                    message: 'The date required and cannot be empty.'
                }
            }
        },
        Hours: {
            message: 'The paid hours is not valid',
            validators: {
                notEmpty: {
                    message: 'The paid hours required and cannot be empty.'
                }
            }
        }
    }
});
//PTO-Edit
$('#PTO-Edit').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        E_EmployeeId: {
            message: 'The employee is not valid',
            validators: {
                notEmpty: {
                    message: 'The employee required and cannot be empty.'
                }
            }
        },
        E_Date: {
            message: 'The date is not valid',
            validators: {
                notEmpty: {
                    message: 'The date required and cannot be empty.'
                }
            }
        },
        E_Hours: {
            message: 'The paid hours is not valid',
            validators: {
                notEmpty: {
                    message: 'The paid hours required and cannot be empty.'
                }
            }
        }
    }
});
//CheckSetup
$('#CheckSetup').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        BankName: {
            message: 'The bank name is not valid',
            validators: {
                notEmpty: {
                    message: 'The bank name required and cannot be empty.'
                }
            }
        },
        BankRouteNo: {
            message: 'The bank route number is not valid',
            validators: {
                notEmpty: {
                    message: 'The bank route number required and cannot be empty.'
                }
            }
        },
        BankAccountNo: {
            message: 'The bank account number is not valid',
            validators: {
                notEmpty: {
                    message: 'The bank account number required and cannot be empty.'
                }
            }
        },
        CurrentCheckNo: {
            message: 'The current check number is not valid',
            validators: {
                notEmpty: {
                    message: 'The current check number required and cannot be empty.'
                }
            }
        }
    }
});
//Help-Add
$('#Help-Add').bootstrapValidator({
    message: 'This value is not valid',
    live: 'enabled',
    fields: {
        Title: {
            message: 'The title is not valid',
            validators: {
                notEmpty: {
                    message: 'The title required and cannot be empty.'
                }
            }
        },
        Type: {
            message: 'The type is not valid',
            validators: {
                notEmpty: {
                    message: 'The type required and cannot be empty.'
                }
            }
        },
        Content: {
            message: 'The content is not valid',
            validators: {
                notEmpty: {
                    message: 'The content required and cannot be empty.'
                }
            }
        }
    }
});