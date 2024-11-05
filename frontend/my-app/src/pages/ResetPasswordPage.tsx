import ResetPasswordForm from "../components/auth/ResetPasswordForm";
import FormPageWrapper from "../components/FormPageWrapper";
function ResetPasswordPage() {
    return(
        <FormPageWrapper title="reset-password">
            <ResetPasswordForm/>
        </FormPageWrapper>
    );
}

export default ResetPasswordPage;