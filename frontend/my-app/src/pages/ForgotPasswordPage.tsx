import ForgotPasswordForm from "../components/auth/ForgotPasswordForm";
import FormPageWrapper from "../components/FormPageWrapper";
function ForgotPasswordPage() {
    return(
        <FormPageWrapper title="reset-password" showHeader={true}>
            <ForgotPasswordForm/>
        </FormPageWrapper>
    );
}

export default ForgotPasswordPage;