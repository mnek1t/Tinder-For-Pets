import ForgotPasswordForm from "../components/ForgotPasswordForm";
import FormPageWrapper from "../components/FormPageWrapper";
function ForgotPasswordPage() {
    return(
        <FormPageWrapper title="reset-password">
            <ForgotPasswordForm/>
        </FormPageWrapper>
    );
}

export default ForgotPasswordPage;