import React, { ReactNode } from 'react';
import '../styles/form.css';

export interface ModalProps {
    children: ReactNode;
    isOpen: boolean;
    onClose?: () => void;
}

function Modal(props: ModalProps) {
    if (!props.isOpen) return null;

    return (
        <div className="modal-overlay" >
            <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                {props.children}
            </div>
        </div>
    );
}

export default Modal;
