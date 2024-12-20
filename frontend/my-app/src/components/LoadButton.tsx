import { CircularProgress } from '@mui/material';
interface LoadButtonProps {
    innertText: string,
    loading: boolean
}

export default function LoadButton(props: LoadButtonProps) {
    return(
        <div>
            {props.loading ? (
                <CircularProgress sx={{color: "#864c4c" }} size={32} /> 
            ) : (
                <button className="load__button" type='submit' disabled={props.loading}>
                    {props.innertText}
                </button>
            )}
        </div>
    );
}