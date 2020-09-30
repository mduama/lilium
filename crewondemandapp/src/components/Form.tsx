import React from "react";
import axios from "axios";
import { CrewList } from "./CrewList";
import { ICrewMemberProps } from "./CrewMember";
import moment from 'moment';

interface IFormProps {}

interface IFormState {
    location: string,
    departureDate: string,
    returnDate: string,
    crewList: ICrewMemberProps[],
    loaded: boolean
}

const utcFormat = "YYYY-MM-DD HH:mm";

class Form extends React.Component<IFormProps, IFormState> {

    private initialState = {
        location : "",
        departureDate: moment().format(utcFormat),
        returnDate: moment().format(utcFormat),
        crewList: [],
        loaded: false,
    }

    constructor(props: IFormProps) {
        super(props);
        moment().format();
        this.state = this.initialState;
    }

    public render(): JSX.Element {
        return (
            <div className="formContainer">
                <form className="form-horizontal">
                    <div className="form-group">
                        <label className="control-label col-sm-4" htmlFor="location">Location:</label>
                        <div className="col-sm-10">
                            <input type="text" className="formControl" id="location" value={this.state.location} onChange={this.onLocationChanged}/>
                        </div>
                    </div>
                    <div className="form-group">
                        <label className="control-label col-sm-4" htmlFor="departureDate">Departure Date: </label>
                        <div className="col-sm-10">
                            <input type="text" className="formControl" id="departureDate" value={this.state.departureDate} onChange={this.onDepartureDateChanged} />
                        </div>
                    </div>
                    <div className="form-group">
                        <label className="control-label col-sm-4" htmlFor="returnDate">Return Date: </label>
                        <div className="col-sm-10">
                            <input type="text" className="formControl" id="returnDate" value={this.state.returnDate} onChange={this.onReturnDateChanged} />
                        </div>
                    </div>
                    <div className="form-group">
                        <div className="col-sm-offset-2 col-sm-10">
                            <button type="submit" className="btn btn-outline-primary" onClick={this.onFormSubmit}>Search</button>
                        </div>
                    </div>
                </form>
                {this.renderCrewList()}
            </div>
        )
    }

    private renderCrewList = () : JSX.Element => {
        return this.state.crewList.length > 0 ? (
           <CrewList crewList={this.state.crewList} onSchedule={this.onSchedule} />
        ) : this.getNoResultsMsg();
    }

    private getNoResultsMsg = () => {
        return (this.state.loaded ? <div className="alert alert-danger">Sorry no crew is available with the provided filters.</div> : <></>);
    }

    private onLocationChanged = async (event: any): Promise<any> => {
        this.setState({"location": event.target.value});
    }

    private onDepartureDateChanged = async (event: any): Promise<any> => {
        this.setState({"departureDate": event.target.value});
    }

    private isValidDate = (d: string) => {
        return moment(d, utcFormat, true).isValid();
    }

    private onReturnDateChanged = async (event: any): Promise<any> => {
        this.setState({"returnDate": event.target.value});
    }

    private onFormSubmit = async (event: any): Promise<any> => {
        event.preventDefault();
        if (!this.isValidDate(this.state.departureDate) || !this.isValidDate(this.state.returnDate)
        || moment(this.state.departureDate, utcFormat) > moment(this.state.returnDate, utcFormat)) {
            alert("Dates are not valid");
            this.setState({
                "departureDate": moment().format(utcFormat),
                "returnDate": moment().format(utcFormat)
            });
        } else {
            const response: any = await axios.create({ baseURL: "https://localhost:44399/" }).request({
                url: "api/Crew/getAvailablePilotsByDatesAndLocation?Location="+this.state.location+"&DepartureDateTime="+this.state.departureDate+"&ReturnDateTime="+this.state.returnDate,
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                }
            }).catch(e => console.log(e));
            this.setState({"crewList": response.data, "loaded": true});
        }
    }

    private onSchedule = async (id: number): Promise<any> => {
        await axios.create({ baseURL: "https://localhost:44399/" }).request({
            url: "/api/Crew/scheduleFlight?pilotId="+id,
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            data: {
                DepartureDateTime: moment(this.state.departureDate, utcFormat).toDate(),
                ReturnDateTime: moment(this.state.returnDate, utcFormat).toDate(),
            }
        }).catch(e => console.log(e));
        alert("done!");
        this.setState(this.initialState);
    }
}

export default Form;
