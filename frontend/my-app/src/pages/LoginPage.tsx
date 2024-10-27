import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import LoginForm from '../components/LoginForm';
import FormPageWrapper from '../components/FormPageWrapper';

function LoginPage() {
    const [isModalOpen, setIsModalOpen] = useState(true);
    const navigate = useNavigate();

    function handleModalClose() {
        setIsModalOpen(false);
        setTimeout(() => {
            navigate('/about');
        }, 300);
    }
   
    return (
        <div>
            <FormPageWrapper title="login">
                <LoginForm handleModalClose={handleModalClose} isOpen={isModalOpen}/>
            </FormPageWrapper>
        </div>
    );
}

export default LoginPage;
