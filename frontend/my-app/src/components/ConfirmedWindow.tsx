import confirmLogo from "../assets/confirm.svg"
interface ConfirmedWindowOptions {
    title: string,
    message: string
}

export default function ConfirmedWindow(props : ConfirmedWindowOptions) {
    return ( 
    <div className="email-sent">
        <div className='email-sent-container'>
            <img className="form-image" src={confirmLogo} alt="confirm logo" />
        </div>
        <h2>{props.title}</h2>
        <p className="forgot-password-hint">{props.message}</p>
    </div> );
}