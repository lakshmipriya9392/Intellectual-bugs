// import { or } from 'ip'
import React from 'react'
import Navbar from './../navbar/navbar'
import { motion } from 'framer-motion'
import axios from 'axios'


function signUp() {
    let Validation = () => {
        //The below code is for name validation

        let name = document.querySelector("#name").value
        let nameWarn = document.querySelector("#name-warn")
        let nameCheck = name.trim()
        if (name == "") {
            nameWarn.textContent = "Name cannot be empty"
        }
        else if (nameCheck == "") {
            nameWarn.textContent = "Name cannot contain only spaces"
        }
        else {
            nameWarn.textContent = ""
        }

        //The below code is for e-mail validation

        let emailId = document.querySelector("#e-mail").value
        let eMailWarn = document.querySelector("#e-mail-warn")
        let emailVerify1 = emailId.includes("@")
        if (emailId == "") {
            eMailWarn.textContent = "E-mail cannot be empty"
        }
        else if (!emailVerify1) {
            eMailWarn.textContent = "E-mail must contain @"
        }
        else {
            eMailWarn.textContent = ""
        }

        //The below code is of password validation

        let password1 = document.querySelector("#password").value
        let password2 = document.querySelector("#confirmPassword").value
        let password1Length = password1.length
        let passwordWarn = document.querySelector("#password-warn")
        let confirmWarn = document.querySelector("#confirm-warn")

        if (password1Length < 8) {
            passwordWarn.textContent = "Your password must be 8 chacracters long"
        }

        else if ((!(password1.includes("1"))) &&
            (!(password1.includes("2"))) &&
            (!(password1.includes("3"))) &&
            (!(password1.includes("4"))) &&
            (!(password1.includes("5"))) &&
            (!(password1.includes("6"))) &&
            (!(password1.includes("7"))) &&
            (!(password1.includes("8"))) &&
            (!(password1.includes("9"))) &&
            (!(password1.includes("0")))
        ) {
            passwordWarn.textContent = "Your password must contain numbers"
        }

        else if (password1 !== password2) {
            confirmWarn.textContent = "Both the password and confirm password are not equal"
        }

        else {
            passwordWarn.textContent = ""
            confirmWarn.textContent = ""
        }

        //The final submitting when everthing is fine
        let proceedWarn = document.querySelector("#proceed-warn")
        const url = 'https://localhost:5001/user/signup'
        let details = {
            name: name,
            emailId: emailId,
            password: password1
        }

        if (
            nameWarn.textContent == "" &&
            eMailWarn.textContent == "" &&
            confirmWarn.textContent == "" &&
            passwordWarn.textContent == ""
        ) {
            proceedWarn.textContent = ""
            // console.log(details)
            document.querySelector("#name").value = ""
            document.querySelector("#e-mail").value = ""
            document.querySelector("#password").value = ""
            document.querySelector("#confirmPassword").value = ""

          axios.post(url, details).then(res => {
              console.log(res)
          }).catch(err => {
              console.log(err)
          })
            // window.location.replace('/courses');
        }
        else {
            proceedWarn.textContent = "Please complete the form to continue"
        }

    }

    let Clear = () => {
        document.querySelector("#name").value = ""
        document.querySelector("#e-mail").value = ""
        document.querySelector("#password").value = ""
        document.querySelector("#confirmPassword").value = ""

        let nameWarn = document.querySelector("#name-warn")
        let eMailWarn = document.querySelector("#e-mail-warn")
        let passwordWarn = document.querySelector("#password-warn")
        let confirmWarn = document.querySelector("#confirm-warn")
        let proceedWarn = document.querySelector("#proceed-warn")
        nameWarn.textContent = ""
        eMailWarn.textContent = ""
        confirmWarn.textContent = ""
        passwordWarn.textContent = ""
        proceedWarn.textContent = ""
    }



    return (
        <div
            className='w-screen h-screen overflow-hidden'
        >
            <motion.div
                initial={{ marginTop: '-100vh' }}
                animate={{ marginTop: 0 }}
                transition={{ delay: 0.5, type: 'spring', stiffness: 250 }}
            >
                <Navbar />
                <div className="flex justify-center">

                    <form action="" className="text-sm shadow-2xl w-11/12 md:w-5/12 h-auto mt-16 mb-2 pb-5 rounded-2xl border-4 flex justify-center flex-col items-center">
                        <div className="text-2xl my-5">Sign Up</div>

                        <input type="text" name="" id="" className="my-4 w-3/4 outline-none border-b-2" placeholder="Your Name" id="name" />
                        <div className="text-red-500 text-xs" id="name-warn"></div>

                        <input type="email" name="" id="" className="my-4 w-3/4 outline-none border-b-2" placeholder="Your e-mail ex: name@example.com" id="e-mail" />
                        <div className="text-red-500 text-xs" id="e-mail-warn"></div>

                        <input type="password" name="" id="" className="my-4 w-3/4 outline-none border-b-2" placeholder="Password" id="password" />
                        <div className="text-red-500 text-xs" id="password-warn"></div>

                        <input type="password" name="" id="" className="my-4 w-3/4 outline-none border-b-2" placeholder="Confirm Password" id="confirmPassword" />
                        <div className="text-red-500 text-xs" id="confirm-warn"></div>

                        <div className="flex justify-between w-full px-16 pt-6 pb-4">
                            <button type="button" className="text-xs md:text-sm border-2 border-black outline-none px-2 py-1 rounded-lg"
                                onClick={Clear}>Clear All</button>
                            <button type="button" className="text-xs md:text-sm border-2 border-black outline-none px-2 py-1 rounded-lg"
                                onClick={Validation}
                            >Submit</button>
                        </div>
                        <div className="text-red-500 text-xs" id="proceed-warn"></div>
                    </form>
                </div>
            </motion.div>
        </div>
    )
}

export default signUp