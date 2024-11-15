interface ConfirmationPopupOption {
    question: string,
    description: string,
    handleAction : (event:React.MouseEvent<HTMLButtonElement>) => void
}

export default function ConfirmationPopup(props : ConfirmationPopupOption) {
    return ( 
        <div className="modal">
        <div className="modal_container">
          <h3>{props.question}</h3>
          <p className="modal_description">{props.description}</p>
          <button className="modal_buttonConfirm" onClick={props.handleAction} name="confirm">
            Confirm
          </button>
          <button className="modal_buttonCancel" onClick={props.handleAction} name="cancel">
            Cancel
          </button>
        </div>
      </div> 
    );
}