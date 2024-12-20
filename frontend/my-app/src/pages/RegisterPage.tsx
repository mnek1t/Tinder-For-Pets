import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import RegisterForm from '../components/auth/RegisterForm';
import FormPageWrapper from '../components/FormPageWrapper';
import { IS_VALID_EMAIL } from '../utils/TinderConstants';
import { InvalidFormatError } from '../utils/CustomErrors';
import { register, RegisterCredentials } from '../api/authApi';
import ConfirmedWindow from '../components/ConfirmedWindow';
function RegisterPage() {
    const [error, setError] = useState<Error | null>(null);
    const [isModalOpen, setIsModalOpen] = useState(true);
    const [loading, setLoading] = useState(false);
    const [isEmailSent, setIsEmailsent] = useState<boolean>(false);
    const navigate = useNavigate();

    function handleModalClose(event: React.MouseEvent<HTMLButtonElement>) {
        event.preventDefault();
        setIsModalOpen(false);
        setTimeout(() => {
            navigate("/about"); 
        }, 300);
    }
    
    function handleRegister(registerCredentials: RegisterCredentials) {
        if(!registerCredentials.email.match(IS_VALID_EMAIL)) {
            setError(new InvalidFormatError("Incorrect email format"));
            return;
        }
        setLoading(true);
        register(registerCredentials)
        .then(() => {
            //navigate("/profile/create");
            setIsEmailsent(true);
            setError(null);
        })
        .catch(error => {
            console.error('Register failed:', error);
            setError(error);
        })
        .finally(() => {
            setLoading(false);
        });
    }
    return (
        <div>
            <FormPageWrapper title='Create Account' showHeader={true}>
                {isEmailSent ? (
                    <ConfirmedWindow title="Your confirmation link has been sent!" message="Go to your email and follow the instructions!"/> 
                ) : (
                    <RegisterForm handleRegister={handleRegister} handleModalClose={handleModalClose} isOpen={isModalOpen} error={error} loading={loading}/>
                )}
                
            </FormPageWrapper>
        </div>
    );
}

export default RegisterPage;
