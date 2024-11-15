import ResetPasswordForm from "../components/auth/ResetPasswordForm";
import FormPageWrapper from "../components/FormPageWrapper";
function ResetPasswordPage() {
    return(
        <FormPageWrapper title="reset-password" showHeader={true}>
            <ResetPasswordForm/>
        </FormPageWrapper>
    );
}

export default ResetPasswordPage;