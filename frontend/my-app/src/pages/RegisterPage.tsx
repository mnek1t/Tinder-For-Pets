import { useState } from 'react';
import RegisterForm from '../components/auth/RegisterForm';
import { useNavigate } from 'react-router-dom';
import FormPageWrapper from '../components/FormPageWrapper';

function RegisterPage() {
    const [isModalOpen, setIsModalOpen] = useState(true);
    const navigate = useNavigate();

    function handleModalClose() {
        setIsModalOpen(false);
        setTimeout(() => {
            navigate("/about"); 
        }, 300);
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
