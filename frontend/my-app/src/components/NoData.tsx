import "../styles/no-data.css"
interface NoDataProps {
    imageSrc : string,
    title: string, 
    description: string 
}
export default function NoData(props: NoDataProps) {
    return(
    <>
    <div className='no-data'>
        <img className="no-data-image" src={props.imageSrc} alt='no-data'></img>
        <h3>{props.title}</h3>
        <p className="no-data-description">{props.description}</p>
    </div>
    </>);
}