import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import RegisterForm from '../components/auth/RegisterForm';
import FormPageWrapper from '../components/FormPageWrapper';
import { IS_VALID_EMAIL } from '../utils/TinderConstants';
import { InvalidFormatError } from '../utils/CustomErrors';
import { register, RegisterCredentials } from '../api/authApi';
function RegisterPage() {
    const [error, setError] = useState<Error | null>(null);
    const [isModalOpen, setIsModalOpen] = useState(true);
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

        register(registerCredentials)
        .then(() => {
            navigate("/about");
            setError(null);
        })
        .catch(error => {
            console.error('Register failed:', error);
            setError(error);
        });
    }
    return (
        <div>
            <FormPageWrapper title='Create Account' showHeader={true}>
                <RegisterForm handleRegister={handleRegister} handleModalClose={handleModalClose} isOpen={isModalOpen} error={error}/>
            </FormPageWrapper>
        </div>
    );
}

export default RegisterPage;
