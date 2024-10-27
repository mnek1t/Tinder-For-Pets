import React, { useState } from 'react';
import RegisterForm from '../components/RegisterForm';
import { useNavigate } from 'react-router-dom';
import FormPageWrapper from '../components/FormPageWrapper';

function RegisterPage() {
    const [isModalOpen, setIsModalOpen] = useState(true);
    const navigate = useNavigate();
    function handleModalClose(event: React.MouseEvent<HTMLButtonElement>) {
        event.preventDefault();
        event.target = event.currentTarget;
        console.log(event.currentTarget)
        
        setTimeout(() => {
            setIsModalOpen(true);
        }, 300);
        navigate("/about");
    }

    return (
        <div>
            <FormPageWrapper title='Create Account'>
                <RegisterForm handleModalClose={handleModalClose} isOpen={isModalOpen}/>
            </FormPageWrapper>
        </div>
    );
}

export default RegisterPage;
