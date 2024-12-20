import ForgotPasswordForm from "../components/auth/ForgotPasswordForm";
import FormPageWrapper from "../components/FormPageWrapper";
import ConfirmedWindow from "../components/ConfirmedWindow";
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { IS_VALID_EMAIL } from "../utils/TinderConstants";
import { InvalidFormatError } from "../utils/CustomErrors";
import { forgotPassword } from "../api/authApi";

function ForgotPasswordPage() {
    const [error, setError] = useState<Error | null>(null);
    const [loading, setLoading] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(true);
    const [isEmailSent, setIsEmailsent] = useState<boolean>(false);
    const navigate = useNavigate();

    function handleModalClose(event: React.MouseEvent<HTMLButtonElement>) {
        event.preventDefault();
        setIsModalOpen(false);
        setTimeout(() => {
            navigate("/about"); 
        }, 300);
    }

    function handleForgotPassword(email: string) {
        if(!email.match(IS_VALID_EMAIL)) {
            setError(new InvalidFormatError("Incorrect email format"));
            return;
        }
        setLoading(true);
        forgotPassword(email)
        .then(() => {
            setIsEmailsent(true);
            setIsModalOpen(false);
        })
        .catch(error => {
            console.error('Forgot Password request failed:', error);
            setError(error);
        })
        .finally(() => {
            setLoading(false);
        });
    }

    return(
        <>
            <FormPageWrapper title="reset-password" showHeader={true}>
                {isEmailSent ? (
                    <ConfirmedWindow title="Your reset link has been sent!" message="Go to your email and follow the instructions!"/> 
                ) : (
                        <ForgotPasswordForm handleForgotPassword={handleForgotPassword} handleModalClose={handleModalClose} isOpen={isModalOpen} error={error} loading={loading}/>
                )}
            </FormPageWrapper>
        </>
    );
}

export default ForgotPasswordPage;