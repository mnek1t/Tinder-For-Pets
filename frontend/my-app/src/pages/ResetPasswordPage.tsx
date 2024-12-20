import { useState } from 'react';
import ResetPasswordForm from "../components/auth/ResetPasswordForm";
import FormPageWrapper from "../components/FormPageWrapper";
import ConfirmationPopup from "../components/ConfirmationPopup";
function ResetPasswordPage() {
    const [error, setError] = useState<Error | null>(null);
    const [loading, setLoading] = useState(false);
    return(
        <FormPageWrapper title="reset-password" showHeader={true}>
            <ResetPasswordForm/>
        </FormPageWrapper>
    );
}

export default ResetPasswordPage;