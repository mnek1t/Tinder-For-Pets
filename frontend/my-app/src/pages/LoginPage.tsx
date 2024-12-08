import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import LoginForm from '../components/auth/LoginForm';
import FormPageWrapper from '../components/FormPageWrapper';
import {login, LoginCredentials} from "../api/authApi"
import { IS_VALID_EMAIL } from '../utils/TinderConstants';
import { InvalidFormatError } from '../utils/CustomErrors';
function LoginPage() {
    const [isModalOpen, setIsModalOpen] = useState(true);
    const [error, setError] = useState<Error | null>(null);
    const navigate = useNavigate();

    function handleModalClose(event:  React.MouseEvent<HTMLButtonElement>) {
        event.preventDefault();
        setIsModalOpen(false);
        setTimeout(() => {
            navigate('/about');
        }, 300);
    }

    function handleLogin(loginCredentials: LoginCredentials) {
        if(!loginCredentials.email.match(IS_VALID_EMAIL)) {
            setError(new InvalidFormatError("Incorrect email format"));
            return;
        }

        login(loginCredentials)
        .then(() => {
            navigate("/app/profile");
            setError(null);
        })
        .catch((error: Error) => {
           setError(error);
        });
    }
    return (
        <div>
            <FormPageWrapper title="login" showHeader={true}>
                <LoginForm handleLogin={handleLogin} handleModalClose={handleModalClose} isOpen={isModalOpen} error={error}/>
            </FormPageWrapper>
        </div>
    );
}

export default LoginPage;
