import React, { useState, ReactNode } from 'react';
import { useNavigate } from 'react-router-dom';
import Modal from './Modal';
import Header from './Header';

interface FormPageWrapperProps {
    children: ReactNode;
    title: string;
}

function FormPageWrapper({ children, title }: FormPageWrapperProps) {
    const [isModalOpen, setIsModalOpen] = useState(true);
    const navigate = useNavigate();

    function handleModalClose() {
        setIsModalOpen(false);
        setTimeout(() => {
            navigate('/about');
        }, 300);
    }

    function handleNavigate(event: React.MouseEvent<HTMLButtonElement>) {
        const { name } = event.target as HTMLButtonElement;
        navigate(name);
    }

    return (
        <div>
            <Header handleNavigate={handleNavigate} />
            <Modal isOpen={isModalOpen} onClose={handleModalClose}>
                {children}
            </Modal>
        </div>
    );
}

export default FormPageWrapper;
