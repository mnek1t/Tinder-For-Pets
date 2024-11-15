import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import LoginForm from '../components/auth/LoginForm';
import FormPageWrapper from '../components/FormPageWrapper';
import {login, LoginCredentials} from "../api/authApi"

function LoginPage() {
    const [isModalOpen, setIsModalOpen] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    function handleModalClose() {
        setIsModalOpen(false);
        setTimeout(() => {
            navigate('/about');
        }, 300);
    }

    function handleLogin(loginCredentials: LoginCredentials) {
        login(loginCredentials)
        .then(() => {
            navigate("/app/profile");
            setError(null);
        })
        .catch((error: unknown) => {
            console.error('Login failed:', error);
            //setError(error.message);
        });
    }
   
    return (
        <div>
            <FormPageWrapper title="login" showHeader={true}>
                <LoginForm handleLogin={handleLogin} handleModalClose={handleModalClose} isOpen={isModalOpen}/>
            </FormPageWrapper>
        </div>
    );
}

export default LoginPage;
