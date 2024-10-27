import closeIcon from "../assets/close-icon.svg";

export default function CloseFormButton({handleClose = (e :  React.MouseEvent<HTMLButtonElement>) => {}}) {
    return(
        <button className='close__button' name='close_modal' onClick={(e) => handleClose(e)}>
            <img className='close__image' src={closeIcon} alt="close icon"/>
        </button>
    );
}