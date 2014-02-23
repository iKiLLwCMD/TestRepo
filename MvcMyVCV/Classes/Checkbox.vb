Public Class BoolValidation
    Public Shared Function ValidateBool(boolToBeTrue As Boolean) As ComponentModel.DataAnnotations.ValidationResult
        If boolToBeTrue Then
            Return ComponentModel.DataAnnotations.ValidationResult.Success
        Else
            Return New ComponentModel.DataAnnotations.ValidationResult("Please Agree to the Terms.")
        End If
    End Function
End Class